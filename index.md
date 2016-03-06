---
layout: home
title: Home
group: top-menu
tagline: Introduction
---
{% include JB/setup %}

<div class="row text-center">
  <div class="col-md-4">
    <p><i class="fa fa-bolt fa-4x"></i></p>
    <h3>Rapid setup</h3>
    <p>The setup of Blitzkrieg solution should not take longer than 20 minutes.</p>
  </div>
  <div class="col-md-4">
    <p><i class="fa fa-balance-scale fa-4x"></i></p>
    <h3>Scalable by design</h3>
    <p>You can adjust the computing power of Blitzkrieg on the fly.</p>
  </div>
  <div class="col-md-4">
    <p><i class="fa fa-smile-o fa-4x"></i></p>
    <h3>Easy to use</h3>
    <p>All functions are wrapped in modern webpage UI and well-designed HTTP API.</p>
  </div>
</div>

<p>&nbsp;</p>

One could think of Blitzkrieg project as “_Artificial Intelligence for games As a Service_” – the intention is to create ease to use, working out-of-the-box solution that will predict best possible moves for any game it is provided with. Moreover, it will be written with scalability in mind, so that it could adjust to the quantity of requests it is receiving or increase its prediction accuracy on the fly, without any effort.

As it is impossible to create game decision making algorithm that plays perfectly every possible game, the responses from Blitzkrieg will be not guaranteed to be best possible. However, they can be considered to be in reasonable boundaries of “good moves”. This will be achieved due to usage of distributed, parallel [Monte Carlo Tree Search](http://jeffbradberry.com/posts/2015/09/intro-to-monte-carlo-tree-search/) as a heart of the decision engine – the algorithm does not need to know theory of the game it is playing, it only needs to know how to play it.

The envisioned user-experience is as follows: customer downloads Blitzkrieg projects and deploys it to cloud provider such as _Azure_ or _AWS_. Then it supplies the service with implementation of the game he wishes to predict moves for. After that, usage is as simple ask asking for best moves in given situation via _HTTP_ API.

# Where to next ?
You can learn more about the project reading [introduction page]({{BASE_PATH}}/docs/introduction).

You can fork the con on our [GitHub Repository]({{ site.github_url }}).
