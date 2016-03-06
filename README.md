Project Blitzkrieg - Scalable game decision engine
=======

 ![Blitzkrieg logo](/assets/images/logo/blitzkrieg-logo.png)

 [![Stories in Backlog](https://badge.waffle.io/AleksanderGondek/project-blitzkrieg.png?label=backlog&title=Backlog)](https://waffle.io/AleksanderGondek/project-blitzkrieg)
 [![Stories in progress](https://badge.waffle.io/AleksanderGondek/project-blitzkrieg.png?label=In%20Progress&title=In progress)](https://waffle.io/AleksanderGondek/project-blitzkrieg)

One could think of Blitzkrieg project as “_Artificial Intelligence for games As a Service_” – the intention is to create ease to use, working out-of-the-box solution that will predict best possible moves for any game it is provided with. Moreover, it will be written with scalability in mind, so that it could adjust to the quantity of requests it is receiving or increase its prediction accuracy on the fly, without any effort.

As it is impossible to create game decision making algorithm that plays perfectly every possible game, the responses from Blitzkrieg will be not guaranteed to be best possible. However, they can be considered to be in reasonable boundaries of “good moves”. This will be achieved due to usage of distributed, parallel [Monte Carlo Tree Search](http://jeffbradberry.com/posts/2015/09/intro-to-monte-carlo-tree-search/) as a heart of the decision engine – the algorithm does not need to know theory of the game it is playing, it only needs to know how to play it.

The envisioned user-experience is as follows: customer downloads Blitzkrieg projects and deploys it to cloud provider such as _Azure_ or _AWS_. Then it supplies the service with implementation of the game he wishes to predict moves for. After that, usage is as simple ask asking for best moves in given situation via _HTTP_ API.

State of project
=======
Currently the project development has just begun.

Branches description
=======
During the development of this project there are going to be three main branches:

* __main__ - this branch signifies "production-ready" code or while before first release, code that has already been checked and is working as intended.
* __main-development__ - this branch signifies code that is not ready for "production" usage, or was not yet confirmed to be working as expected. Before first release this is going to be main working branch.
* __gh-pages__ - dedicated branch for hosting web pages. Here all important documentation will be written.

Technology stack
=======
This project is proudly developed using [.NET Core](https://dotnet.github.io/) and thus is available for use across platforms (_Windows_ and _Linux_ alike). Hopefully this makes using this project in cloud environment even easier. It is also heavily using [Orleans Framework](http://dotnet.github.io/orleans/), which greatly helps in distributing and parallelizing the code.

License
=======
Blitzkrieg is licensed under the [Apache 2.0 license](https://github.com/AleksanderGondek/project-blitzkrieg/blob/master/LICENSE).

Disclaimer:
=======
This project is developed in part for my Master Thesis “_Distributed Application for simulating selected game mechanics based on Orleans framework_” for [Gdańsk University of Technology - Faculty of Electronics, Telecommunications and Informatics](http://www.pg.gda.pl/en/index.php/faculties/weti).

The thesis supervisor is [Dr. Krzysztof Manuszewski](http://pg.edu.pl/e105b88b3e_krzysztof.manuszewski).
