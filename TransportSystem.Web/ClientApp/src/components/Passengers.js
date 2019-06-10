import React from 'react';
import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';
import {actionCreators} from '../store/Passengers';
import PassengersShow from './PassengersShow';

const selectItems = [
    "Selection algorithm depending on the average level of neighbors",
    "Selection algorithm depending on the deviation from the level of neighbors",
    "Selection algorithm using reinforcement learning data"
];

const types = [
    {
        name: "Selection algorithm depending on the average level of neighbors",
        type: "Average"
    },
    {
        name: "Selection algorithm depending on the deviation from the level of neighbors",
        type: "Deviation"
    },
    {
        name: "Selection algorithm using reinforcement learning data",
        type: "QLearning"
    }];

const transportTypes = ["Car", "Bus"];

class Passengers extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            columns: 3,
            rows: 3,
            select: selectItems[0],
            algorithmType: types[0].type,
            passengers: []
        }
    }

    getInitForm() {
        return (
            <div>
                <h3>Init parameters</h3>
                <div>
                    <p>
                        Number of columns: <input type="number" name="columnCount" onChange={({target}) => {
                        this.setState({columns: target.value})
                    }} value={this.state.columns} step="1" min="1" max="5"/>
                    </p>
                    <p>
                        Number of rows: <input type="number" name="rowCount" onChange={({target}) => {
                        this.setState({rows: target.value})
                    }} value={this.state.rows} step="1" min="1" max="5"/>
                    </p>
                    <p>
                        Transfer function:
                        <select onChange={({target}) => {
                            this.setState({
                                select: target.value,
                                algorithmType: types.find(x => x.name === target.value).type
                            })
                        }} value={this.state.select} name="func">
                            <option disabled>Select transfer function</option>
                            {selectItems.map(title => {
                                return <option value={title}>{title}</option>
                            })}
                        </select>
                    </p>
                    <button className="btn btn-success"
                            onClick={() => this.props.setMainProperties(this.state)}>Create
                    </button>
                </div>
            </div>
        );
    }

    getPassengersParametersForm() {
        let passengers = this.props.smoStep.passengers;
        let rows = [];
        let row = [];
        for (let i = 0; i < passengers.length; i++) {
            let passenger = passengers[i];
            let currentIndex = i;
            if (row.length === this.props.columns) {
                rows.push((<div className="passengersRow">{row}</div>));
                row = [];
            }
            row.push((
                <div className="cell">
                    <h4>
                        Passenger {passenger.id}
                    </h4>
                    <p>
                        Satisfaction: <input type="number" name="newPassengers[@currentIndex].Satisfaction"
                                             onChange={({target}) => {
                                                 let newParameters = {...passenger, satisfaction: target.value};
                                                 this.props.updatePassenger({
                                                     passengerIndex: currentIndex,
                                                     newParameters
                                                 });
                                             }
                                             }
                                             value={passenger.satisfaction}
                                             step="0.01" min="0" max="1"/>
                    </p>
                    <p>
                        Quality of services: <input type="number" name="newPassengers[@currentIndex].QualityCoefficient"
                                                    onChange={({target}) => {
                                                        let newParameters = {...passenger, quality: target.value};
                                                        this.props.updatePassenger({
                                                            passengerIndex: currentIndex,
                                                            newParameters
                                                        });
                                                    }
                                                    }
                                                    value={passenger.quality}
                                                    step="0.01" min="0" max="1"/>
                    </p>
                    <p>
                        <select onChange={({target}) => {
                            let newParameters = {...passenger, transportType: target.value};
                            this.props.updatePassenger({passengerIndex: currentIndex, newParameters});
                        }}
                                value={passenger.transportType}
                                name="newPassengers[@currentIndex].TransportType">
                            <option disabled>Select the transport</option>
                            {transportTypes.map(x => (<option value={x}>{x}</option>))}
                        </select>
                    </p>
                </div>));
        }
        if (row.length !== 0)
            rows.push((<div className="passengersRow">{row}</div>));

        return (<div>
            {rows}
            <button className="btn btn-success" onClick={() => this.props.setInteractiveMode(this.state)}>Submit
            </button>
        </div>);
    }

    render() {
        let mainPart = (
            <div>
                <h1>Passengers</h1>
                <button className="btn btn-success" onClick={this.props.setInitState}>Reset</button>
            </div>);
        let initForm = (this.props.initState)
            ? this.getInitForm()
            : <div/>;
        let passengersParametersForm = (!this.props.initState && !this.props.interactiveMode)
            ? this.getPassengersParametersForm()
            : <div/>;
        let passengersShowInfo = (this.props.interactiveMode) ?
            (<div>
                {<PassengersShow passengers={this.props.smoStep.passengers}
                                 columns={this.props.columns}/>}
                <button className="btn btn-success" onClick={this.props.getNextStep}>Next</button>
            </div>)
            : <div/>;

        return (<div>
            {mainPart}
            {initForm}
            {passengersParametersForm}
            {passengersShowInfo}
        </div>);
    }
}

export default connect(
    state => state.passengers,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Passengers);
