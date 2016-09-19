"use strict";

angular.module("blitzkrieg", ["nywton.chessboard", "angular-loading-bar", "ngAnimate"])
.config(['nywtonChessboardConfigProvider', function nywtonChessConfigConfig(chessboardProvider) {
    chessboardProvider.pieceTheme('/Content/chessboard-js/img/chesspieces/wikipedia/{piece}.png')
    .position('start');
}])
.config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.latencyThreshold = 10;
    cfpLoadingBarProvider.spinnerTemplate = "<div class=\"screen-centered\"><span class=\"fa fa-spinner fa-spin fa-3x\"></div>";
}])
.controller("chessGameCtrl",
    function ($scope, $http, $q, $interval) {
        // Flags
        $scope.gameInterval = null;
        $scope.apiCallUnderway = false;
        $scope.gameTickInterval = 10000; // 10 seconds

        // Possible execution types
        $scope.executionTypesAvailable = [
            "Serial-With-Uct", "Root-Parallelization-With-Uct", "Mcts-Shared-Tree-With-Utc"
        ];

        // Request payload
        $scope.gameTickRequest = {
            ExectutionType: "Root-Parallelization-With-Uct",
            Workers: 10,
            MaximumIterations: 40,
            MaxiumumSimulations: 40
        }
        
        // Chess Game Holder
        $scope.gameState = {
            raw: undefined,
            processed: {}
        };

        // Helper methods
        $scope.getPlayerId = function(listOfPlayers, player) {
            // If player is first on the list of all players, assume its the white player
            if (listOfPlayers.indexOf(player) === 0) {
                return "w";
            }

            // Else it is the black player
            return "b";
        };
        $scope.getChessPieceId = function(playerId, pieceEnum) {
            if (pieceEnum === 0) {
                return playerId+ "P";
            }
            if (pieceEnum === 1) {
                return playerId + "N";
            }
            if (pieceEnum === 2) {
                return playerId + "B";
            }
            if (pieceEnum === 3) {
                return playerId + "R";
            }
            if (pieceEnum === 4) {
                return playerId + "Q";
            }
            if (pieceEnum === 5) {
                return playerId + "K";
            }
        };
        $scope.processGameState = function(gameState) {
            $scope.gameState.raw = JSON.parse(gameState["GameState"]);

            if (_.isUndefined($scope.gameState.raw) ||
                _.isUndefined($scope.gameState.raw["GameBoard"]) ||
                _.isUndefined($scope.gameState.raw["AllPlayers"])) {
                return;
            }

            $scope.gameState.processed = {};
            _.each($scope.gameState.raw.GameBoard,
                function(boardInformation, boardPosition) {
                    var playerId = $scope.getPlayerId($scope.gameState.raw["AllPlayers"], boardInformation.Owner);
                    $scope.gameState.processed[boardPosition.toLowerCase()] = $scope.getChessPieceId(playerId, boardInformation.ChessPiece);
                });
        };

        $scope.startGame = function() {
            if ($scope.gameInterval === null) {
                $scope.gameTick();
                $scope.gameInterval = $interval($scope.gameTick, $scope.gameTickInterval);
           } else {
               $interval.cancel($scope.gameInterval);
               $scope.gameInterval = null;
           }
        };

        $scope.gameTick = function () {
            var deferred = $q.defer();

            $scope.apiCallUnderway = true;
            $http({
                    method: "POST",
                    url: "/ChessGame/GameTick",
                    data: angular.toJson($scope.gameTickRequest),
                    timeout: 600000 // 10 minutes
                })
                .then(function(response) {
                        $scope.processGameState(response.data);
                        $scope.boardA.position($scope.gameState.processed);
                        $scope.apiCallUnderway = false;
                        deferred.resolve();
                    },
                    function(resposne) {
                        // Error
                        $scope.apiCallUnderway = false;
                        deferred.reject();
                    });

            return deferred.promise;
        };
    });
