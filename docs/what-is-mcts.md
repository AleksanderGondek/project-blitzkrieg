---
layout: page
title: "What Is Mcts"
group: "side-menu"
description: ""
---
{% include JB/setup %}

### Algorithm overview

We can describe Monte-Carlo Tree Search as a heuristic algorithm used to make most optimal decisions in given decision processes. It is most notably used to predict the best moves in general game playing. Aforementioned algorithm is becoming very popular and there are very good reasons for it.
First of all, we need to mention that for a very long period of time, another method (alfa-beta pruning) was prominent in the field of decision making algorithms for games – however this has changed in 2006 when MCTS was introduced in the field of playing Go. Since then MCTS is only getting more and more popular and dominant – that is because, compared to alfa-beta pruning, MCTS needs only implemented game mechanics to work, which is a huge advantage – MCTS can play games without developed theory, even (or mostly) general gaming. Moreover, game in MCTS is played in asymmetric manner, where algorithm is focusing only on the most promising parts. Lastly MCTS can be safely interrupted at any time of its execution and still return valid game move – it’s value depends on time spend on its prediction.
Due to mentions advantages over alfa-beta search, MCTS is getting widely adopted both in scientific projects as in computer games (i.e. many “bots” created to play online games rely on MCTS).

### Serial implementation overview

![alt tag](https://raw.github.com/AleksanderGondek/project-blitzkrieg/gh-pages/assets/images/illustrations/SchemeOfMCTS.png)

Monte Carlo Tree Search is algorithm based on semi-randomized exploration on given search space. It uses the results of previous explorations to continuously build game tree, due to which it become increasingly better at given game, with each exploration performed. This also increased the accuracy of estimating which move is better.
Each run of the algorithm that is used to expand the game tree is called “playout”. In each playout the algorithm is playing the game to the end (or timeout) and then it uses the game playout outcome to weight the tree nodes so that in future better nodes (moves) ale more likely to be selected.

We can differentiate four different rounds of MCTS playout:

1. **Selection step** - the algorithm is selecting successive nodes, starting from the tree root. It basically takes the best known moves from the start of the game tree, to its leaf. How this selection is performed depends on chosen strategy – the main difficulty lies in balancing both three exploration and xploitation of previously added values.
The most popular and first formula for achieving such balance is called “Upper Confidence Bound  applied to tree” (UTC).
2. **Expansion step** - if the chosen tree leaf (move) ends the game, the playout jumps to backpropagation step. Otherwise it chooses one or more available moves and adds them as childes of the node.
3. **Simulation step** - the game is simulated and played to the end from each added new node.
4. **Backpropagation step** - After the simulation is done, the outcome of playout is propagated to each node from the last
one to the root.

As it was mention before, you can notice that the playout can be stopped at any time, however the
longer it plays the better it becomes in choosing the best possible moves.

### Parallel implementation overview

![alt tag](https://raw.github.com/AleksanderGondek/project-blitzkrieg/gh-pages/assets/images/illustrations/SchemeOfParallelMCTS.png)

Broadly speaking we can distinguish three ways of parallelizing MCTS algorithm: leaf parallelization, root parallelization and tree parallelization.

#### Using Leaf Parallelization

This way of performing MCTS parallelization may be perceived as most intuitive one. It was proposed by Cazenave and Jouandeau and it takes the advantage of previously mention fact that the playout step 3 (simulation) is independent. In short, the MCTS runs as a serial code, until
entering simulation step. There, instead choosing only one node to play to the end (or many in serial manner) we are running all possible nodes in parallel. Due to this approach we will be propagating all possible outcomes, instead of one. This seems easy and does not require any mutexes – however it is highly unpredictable, takes at average more time playing out only one game and may lead to wasting computational power.

#### Using Root Parallelization

This is another way of MCTS parallelization proposed by Cazenave. In simple terms it consists of building multiple game trees, one for each thread. They do not share information with each other. At the end, after all playouts are finished, all those game trees are merged and based on
the merged tree the best move is selected. This approach requires very scare communication and does not suffer from some of the Leaf Parallelization issues.

>
> Rest to be described.
>
