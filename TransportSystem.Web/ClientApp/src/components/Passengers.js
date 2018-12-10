import React from 'react';
import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';
import {actionCreators} from '../store/Passengers';
import PassengersShow from './PassengersShow';
import {createRandomPassengers} from '../helpers/passengersHelper';

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

const transportTypes = ["car", "bus"];

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

    getFirstForm() {
        return (
            <div>
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
                            this.setState({select: target.value, algorithmType: types.find(x => x.name === target.value).type})
                        }} value={this.state.select} name="func">
                            <option disabled>Select transfer function</option>
                            {selectItems.map(title => {
                                return <option value={title}>{title}</option>
                            })}
                        </select>
                    </p>
                    <input className="btn btn-success" type="button" onClick={() => {
                        let passengers = createRandomPassengers(this.state.rows, this.state.columns, transportTypes);
                        console.log('=11111111111111111111');
                        console.log(passengers);
                        this.savePassengers(passengers);
                        this.props.setMainProperties(this.state);
                    }} value="Create"/>
                </form>
            </div>
        );
    }

    savePassengers(passengers) {
        this.setState({passengers});
    }

    getSecondForm() {
        let passengers = this.state.passengers;

        console.log(this.state.passengers);
        let allPassengersCells = [];
        for (let i = 0; i < this.state.rows; i++) {
            let rows = [];
            for (let j = 0; j < this.state.columns; j++) {
                let passenger = this.state.passengers[i][j];
                let index = {i, j};
                rows.push((
                    <div className="cell">
                        <h4>
                            Passenger {passenger.number}
                        </h4>
                        <p>
                            Satisfaction: <input type="number" name="newPassengers[@i][@j].Satisfaction"
                                                 onChange={({target}) => {
                                                     console.log(Number.parseFloat(target.value));
                                                     console.log(target.value);
                                                     this.state.passengers[index.i][index.j].satisfaction = Number.parseFloat(target.value);
                                                     this.setState(passengers)
                                                 }
                                                 }
                                                 value={passenger.satisfaction}
                                                 step="0.01" min="0" max="1"/>
                        </p>
                        <p>
                            Quality of services: <input type="number" name="newPassengers[@i][@j].QualityCoefficient"
                                                        onChange={({target}) => {
                                                            this.state.passengers[index.i][index.j].quality = Number.parseFloat(target.value);
                                                            this.setState(passengers)
                                                        }
                                                        }
                                                        value={passenger.quality}
                                                        step="0.01" min="0" max="1"/>
                        </p>
                        <p>
                            <select onChange={({target}) => {
                                this.state.passengers[index.i][index.j].transportType = target.value;
                                this.setState(passengers)
                            }}
                                    value={passenger.transportType}
                                    name="newPassengers[@i][@j].TransportType">
                                <option disabled>Select the transport</option>
                                {transportTypes.map(x => (<option value={x}>{x}</option>))}
                            </select>
                        </p>
                    </div>));
            }
            allPassengersCells.push(<div className="passengersRow">{rows}</div>);
        }

        return (<form onSubmit="return false">
            {allPassengersCells}
            <input className="btn btn-success" type="submit" onClick={() => {
                this.props.setInteractiveMode(this.state)
            }} value="Submit"/>
        </form>);
    }

    render() {

        let initForm = (
            <div>
                <h1>Initial parameters</h1>
                <button onClick={this.props.setInitState}>Заново</button>
                {this.getFirstForm()}
            </div>);

        if (this.props.interactiveMode) {
            return (<div>
                {initForm}
                {<PassengersShow passengers={this.props.passengers} 
                                 height={this.props.rows}
                                 width={this.props.columns}/>}
                <button className="btn btn-success" onClick={this.props.getNextStep}>Next</button>
            </div>)
        }

        if (!this.props.initState) {
            return (<div>{initForm}{this.getSecondForm()}</div>)
        }


        console.log(this.props);
        return initForm;
    }
}

export default connect(
    state => state.passengers,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Passengers);
