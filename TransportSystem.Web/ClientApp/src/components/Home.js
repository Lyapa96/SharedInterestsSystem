import React from 'react';
import {connect} from 'react-redux';

const Home = props => (
    <div>
        <h1>Welcome to association scientific web services!</h1>
        <p>
            The current work is devoted to building a model for assessing the satisfaction of passenger service by the
            public transport system. The system is constructed using intelligent agents, whose acting is based on
            self-learning principles. The agents are passengers who depend on transport and can choose between two
            modes: a car or a bus, where their choice of transport mode for the next day is based on their level of
            satisfaction and their neighbors’ satisfaction with the mode they used the day before. The paper considers
            several algorithms of agent behavior, one of which is based on reinforcement learning. Overall, the
            algorithms take into account the history of the agent’s previous trips and the quality of transport
            services. The outcomes could be applied in assessing the quality of the transport system from the point of
            view of passengers.
        </p>
    </div>
);

export default connect()(Home);
