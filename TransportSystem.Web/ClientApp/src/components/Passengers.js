import React from 'react';
import {bindActionCreators} from 'redux';
import {connect} from 'react-redux';
import {actionCreators} from '../store/Passengers';
import PassengersShow from './PassengersShow';

const selectItems = ["Selection algorithm depending on the average level of neighbors", "2", "3"];
const transportTypes = ["car", "bus"];

class Passengers extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            columns: 3,
            rows: 3,
            select: selectItems[0],
            passengers: []
        }
    }
    
     getFirstForm() {
        return (
            <div>
                <form onSubmit="return false">
                    <p>
                        Number of columns: <input type="number" name="columnCount" onChange={({target}) => {this.setState({columns: target.value})}} value={this.state.columns} step="1" min="1" max="5"/>
                    </p>
                    <p>
                        Number of rows: <input type="number" name="rowCount" onChange={({target}) => {this.setState({rows: target.value})}} value={this.state.rows} step="1" min="1" max="5"/>
                    </p>
                    <p>
                        Transfer function: 
                            <select onChange={({target}) => {this.setState({select: target.value})}} value={this.state.select} name="func">
                                <option disabled>Select transfer function</option>
                                {selectItems.map(title => {
                                    return <option value={title}>{title}</option>
                                })}
                            </select>
                    </p>
                    <input className="btn btn-success" type="button" onClick={() => {console.log(this.state);this.props.setMainProperties(this.state); this.savePassengers(this.createRandomPassengers())}} value="Create" />
                </form>
            </div>
        );
    }
    
    createRandomPassengers() {
        let passengers = [];
        for (let i = 0; i < this.state.rows; i++) {
            let currentRow = [];
            for(let j = 0; j < this.state.columns; j++) {
                currentRow.push({
                    number: `${i} ${j}`,
                    satisfaction: Math.random().toFixed(2),
                    quality: Math.random().toFixed(2),
                    transportType: transportTypes[this.randomInteger(0, 1)]
                });
            }
            passengers.push(currentRow)
        }
        
        return passengers;
    }

     randomInteger(min, max) {
        let rand = min - 0.5 + Math.random() * (max - min + 1);
        rand = Math.round(rand);
        return rand;
    }

    savePassengers(passengers){
        this.setState({passengers});
    }

    getSecondForm() {
        let passengers = this.state.passengers;
        
        console.log(this.state.passengers);    
        let array = [];
        for (let i = 0; i < this.state.rows; i++) {
            for(let j = 0; j < this.state.columns; j++) {
                let passenger = this.state.passengers[i][j]; 
                let index = {i,j};
                array.push((<div>

                    <div className="cell">
                        <h4>
                            Passenger {passenger.number}
                        </h4>
                        <p>
                            Satisfaction: <input type="number" name="newPassengers[@i][@j].Satisfaction"
                                                 onChange={({target}) => {
                                                     this.state.passengers[index.i][index.j].satisfaction = target.value;
                                                     this.setState(passengers)
                                                    }
                                                 }
                                                 value={passenger.satisfaction}
                                                 step="0.01" min="0" max="1"/>
                        </p>
                        <p>
                            Quality of services: <input type="number" name="newPassengers[@i][@j].QualityCoefficient"
                                                        onChange={({target}) => {
                                                            this.state.passengers[index.i][index.j].quality = target.value;
                                                            this.setState(passengers)
                                                        }
                                                        }
                                                        value={passenger.quality}
                                                        step="0.01" min="0" max="1"/>
                        </p>
                        <p>
                            <select onChange={({target}) => passengers.transport= target.value} value={passenger.transportType} name="newPassengers[@i][@j].TransportType">
                                <option disabled>Select the transport</option>
                                {transportTypes.map(x => (<option value={x}>{x}</option>))}
                            </select>
                        </p>
                    </div>
                </div>));
            }
        }
        
        return (<form onSubmit="return false">
            {array}
            <input className="btn btn-success" type="submit" onClick={() => {this.props.setInteractiveMode(this.state)}} value="Submit"/>
        </form>);
    }
    
    getThirdForm() {
        
    }

    render() {
        
        let firstForm = (
            <div>
                <h1>Начальные параметры</h1>
                <button onClick={this.props.setInitState}>Заново</button>
                {this.getFirstForm()}
            </div>);
        
        if (this.props.interactiveMode){
            return (<div>
                {firstForm}
                {<PassengersShow passengers={this.props.passengers} height={this.props.rows} width={this.props.columns}/>}
                <button onClick={this.props.getNextStep}>Next</button>
            </div>)
        }

        if (!this.props.initState) {
            return (<div>{firstForm}{this.getSecondForm()}</div>)
        }   
        
        
        console.log(this.props);
        return firstForm;
        }
    }

    export default connect(
    state => state.passengers,
    dispatch => bindActionCreators(actionCreators, dispatch)
    )(Passengers);
