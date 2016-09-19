using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Brokers;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess;
using AleksanderGondek.ProjectBlitzkrieg.Website.Models;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.Website.Controllers
{
    public class ChessGameController : Controller
    {
        private static IDictionary<Guid, ChessGameState> _chessGameStates;

        public ChessGameController()
        {
            if (_chessGameStates == null)
            {
                _chessGameStates = new ConcurrentDictionary<Guid, ChessGameState>();
            }
        }

        // GET: ChessGame
        public ActionResult Play()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GameTick(GameTickRequestModel gameTickRequest)
        {
            if (!IsGameTicketRequestValid(gameTickRequest))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid gameTick request");
            }

            var gameGuid = GetGameGuid();
            var gameState = GetGameState(gameGuid);
            if (!gameState.AvailableActions().Any())
            {
                return Json(new { GameState = gameState.ToJson() }, JsonRequestBehavior.AllowGet);
            }

            var request = new ProcessingRequest()
            {
                GameState = gameState.ToJson(),
                ExectutionType = gameTickRequest.ExectutionType,
                MaximumIterations = gameTickRequest.MaximumIterations,
                MaxiumumSimulations = gameTickRequest.MaxiumumSimulations,
                Workers = gameTickRequest.Workers
            };

            var brokerTest = GrainClient.GrainFactory.GetGrain<IMctsBroker>(gameGuid);
            var nextMove = await brokerTest.GetNextMove(request);

            gameState.PerformAction(nextMove);
            _chessGameStates[gameGuid] = gameState;

            return Json(new {GameState = gameState.ToJson() }, JsonRequestBehavior.AllowGet);
        }

        private bool IsGameTicketRequestValid(GameTickRequestModel gameTickRequest)
        {
            return !string.IsNullOrEmpty(gameTickRequest?.ExectutionType) && 
                gameTickRequest.MaximumIterations >= 0 && 
                gameTickRequest.MaxiumumSimulations >= 0 && 
                gameTickRequest.Workers >= 0;
        }

        private ChessGameState GetGameState(Guid gameId)
        {
            if (!_chessGameStates.ContainsKey(gameId))
            {
                var newGameState = new ChessGameState();
                newGameState.Initialize();
                newGameState.IsValid();

                _chessGameStates.Add(gameId, newGameState);
            }

            ChessGameState gameState;
            _chessGameStates.TryGetValue(gameId, out gameState);
            return gameState;
        }

        private Guid GetGameGuid()
        {
            if (Request.Cookies["gameId"] != null)
            {
                return Guid.Parse(Request.Cookies["gameId"].Value);
            }

            var guid = Guid.NewGuid();
            Response.Cookies.Add(new HttpCookie("gameId", guid.ToString()));
            return guid;
        }
    }
}