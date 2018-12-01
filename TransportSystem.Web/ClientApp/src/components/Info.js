import React from 'react';
import { connect } from 'react-redux';

const Info = props => (
    <div>
    <h2>The program of simulation of passengers satisfaction with transport services using various mathematical approaches in systems with shared interests.</h2>

    <p>The program allows you to create a system of passengers with configurable initial parameters, set the transfer function of the choice of transport for passengers and observe the behavior of a particular passenger and the entire system as a whole, depending on the initial conditions of the system and the interactions that have occurred.</p>
</div>
);

export default connect()(Info);