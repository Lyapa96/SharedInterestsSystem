import React from 'react';
import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';
import {actionCreators} from '../store/Passengers';
import PassengersShow from './PassengersShow';
import {createRandomPassengers, createRandomPassengers2} from '../helpers/passengersHelper';

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
            algorithmType: types[0],
            passengers: []
        }
    }

    getInitForm() {
        return (
            <div>
                <h3>Init parameters</h3>
                <form onSubmit="return false">
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
                    <input className="btn btn-success" type="button" onClick={() => {
                        let passengers = createRandomPassengers2(this.state.rows, this.state.columns, transportTypes);
                        this.savePassengers(passengers);
                        this.props.setMainProperties(this.state);
                    }} value="Create"/>
                </form>
            </div>
        );
    }

    savePassengers = passengers => this.setState({passengers});

    getPassengersParametersForm() {
        let passengers = this.state.passengers;
        let allPassengersCells = [];
        let rowsCount = this.state.rows;
        for (let i = 0; i < rowsCount; i++) {
            let rows = [];
            for (let j = 0; j < this.state.columns; j++) {
                let currentIndex = rowsCount * i + j;
                let passenger = passengers[currentIndex];
                rows.push((
                    <div className="cell">
                        <h4>
                            Passenger {passenger.id}
                        </h4>
                        <p>
                            Satisfaction: <input type="number" name="newPassengers[@currentIndex].Satisfaction"
                                                 onChange={({target}) => {
                                                     console.log(Number.parseFloat(target.value));
                                                     console.log(target.value);
                                                     this.state.passengers[currentIndex].satisfaction = Number.parseFloat(target.value);
                                                     this.setState(passengers)
                                                 }
                                                 }
                                                 value={passenger.satisfaction}
                                                 step="0.01" min="0" max="1"/>
                        </p>
                        <p>
                            Quality of services: <input type="number" name="newPassengers[@currentIndex].QualityCoefficient"
                                                        onChange={({target}) => {
                                                            this.state.passengers[currentIndex].quality = Number.parseFloat(target.value);
                                                            this.setState(passengers)
                                                        }
                                                        }
                                                        value={passenger.quality}
                                                        step="0.01" min="0" max="1"/>
                        </p>
                        <p>
                            <select onChange={({target}) => {
                                this.state.passengers[currentIndex].transportType = target.value;
                                this.setState(passengers)
                            }}
                                    value={passenger.transportType}
                                    name="newPassengers[@currentIndex].TransportType">
                                <option disabled>Select the transport</option>
                                {transportTypes.map(x => (<option value={x}>{x}</option>))}
                            </select>
                        </p>
                    </div>));
            }
            allPassengersCells.push(<div className="passengersRow">{rows}</div>);
        }

        return (<div>
            {allPassengersCells}
            <button className="btn btn-success" onClick={() => this.props.setInteractiveMode(this.state)}>Submit2</button>
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
