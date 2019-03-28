import React from 'react';
import {connect} from 'react-redux';
import SmoCharts from './SmoCharts';
import PassengersShow from "./PassengersShow";
import {actionCreators} from '../store/Passengers';
import {createRandomPassengers} from '../helpers/passengersHelper';
import {bindActionCreators} from "redux";

class Smo extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            passengersCount: 0,
            channelCount: 0,
            isAdditional: false,
            isInteractiveMode: false,
            passengers: createRandomPassengers(3, 3, ['Car', 'Bus'])
        }
    }

    render() {
        let mainPart = (
            <div>
                <h1>Queueing theory</h1>
                <button className="btn btn-success" onClick={this.props.setInitState}>Reset</button>
            </div>);
        let initialFrom = (
            <div>
                <h3>Init parameters</h3>
                <form>
                    <p>
                        Number of passengers: <input type="number" name="columnCount" onChange={({target}) => {
                        this.setState({passengersCount: target.value})
                    }} value={this.state.passengersCount} step="1" min="1" max="5"/>
                    </p>
                    <p>
                        Number of channel: <input type="number" name="columnCount" onChange={({target}) => {
                        this.setState({channelCount: target.value})
                    }} value={this.state.channelCount} step="1" min="1" max="5"/>
                    </p>
                    <input className="btn btn-success" type="button" onClick={() => {
                        this.setState({isAdditional: true})
                    }} value="Create"/>
                </form>
            </div>);

        let chart = (this.state.isAdditional) ? <SmoCharts/> : <div/>;
        // let button = (this.state.isAdditional)
        //     ? (<button className="btn btn-success" onClick={this.setState({isInteractiveMode: true})}>Start visualizing
        //         choice</button>)
        //     : <div/>;
        let passengersShowInfo = (this.state.isAdditional) ?
            (<div>
                {<PassengersShow passengers={this.state.passengers}
                                 height={3}
                                 width={3}/>}
                <button className="btn btn-success" onClick={() => 
                    this.setState({passengers: createRandomPassengers(3, 3, ['Car', 'Bus'])})
                }>Next</button>
            </div>)
            : <div/>;

        return (
            <div>
                {mainPart}
                {initialFrom}
                {chart}
                {/*{button}*/}
                {passengersShowInfo}
            </div>)
    }
}

export default connect(
    state => state.passengers,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Smo);