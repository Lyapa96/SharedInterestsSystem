import React from 'react';
import {connect} from 'react-redux';
import {actionCreators} from '../store/Transports';
import {bindActionCreators} from "redux";
import SmoPassengersView from "./SmoPassengersView";
import SmoResultChart from "./SmoResultChart";
import TransportsCountChart from "./TransportsCountChart";
import TransportsQualityChart from "./TransportsQualityChart";

class Transports extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            passengersCount: 30,
            columns: 10,
            neighboursCount: 7,
            availableTransportTypes: [
                {type: "Car", active: true},
                {type: "Bus", active: true},
                {type: "Bike", active: false},
                {type: "Subway", active: false},
                {type: "Tram", active: false}
            ]
        }
    }

    getDataFroCountChart(iterationResults) {
        let result = this.state.availableTransportTypes
            .filter(x => x.active)
            .map(x => {
                return {
                    name: x.type,
                    iterationsData: iterationResults.map(x => 0)
                };
            });

        for (let i = 0; i < iterationResults.length; i++) {
            for (let passenger of iterationResults[i].passengers) {
                let type = result.find(x => x.name === passenger.transportType);
                type.iterationsData[i]++;
            }
        }

        return result;
    }

    getDataForQualityChart(iterationResults) {
        let result = this.state.availableTransportTypes
            .filter(x => x.active)
            .map(x => {
                return {
                    name: x.type,
                    iterationsData: iterationResults.map(x => {
                        return {
                            count: 0,
                            commonSatisfaction: 0
                        }
                    })
                };
            });

        for (let i = 0; i < iterationResults.length; i++) {
            for (let passenger of iterationResults[i].passengers) {
                let type = result.find(x => x.name === passenger.transportType);
                type.iterationsData[i].count++;
                type.iterationsData[i].commonSatisfaction += passenger.satisfaction;
            }
        }

        return result.map(x => {
            x.iterationsData = x.iterationsData.map(data => {
                if (data.count === 0)
                    return 0;
                return data.commonSatisfaction / data.count;
            });
            return x;
        });
    }

    render() {
        let initialFrom = (
            <div>
                <h3>Init parameters</h3>
                <form>
                    <p>
                        Number of passengers: <input disabled={!this.props.isInitState} type="number" name="columnCount"
                                                     onChange={({target}) => {
                                                         this.setState({passengersCount: Number(target.value)})
                                                     }} value={this.state.passengersCount} step="1" min="1" max="50"/>
                    </p>
                    <p>
                        Columns: <input disabled={!this.props.isInitState} type="number" name="columnCount"
                                        onChange={({target}) => {
                                            this.setState({columns: Number(target.value)})
                                        }} value={this.state.columns} step="1" min="1" max="10"/>
                    </p>
                    <p>
                        Number of neighbours: <input disabled={!this.props.isInitState} type="number" name="columnCount"
                                                     onChange={({target}) => {
                                                         this.setState({neighboursCount: Number(target.value)})
                                                     }} value={this.state.neighboursCount} step="1" min="2" max="10"/>
                    </p>
                    {this.state.availableTransportTypes.map((item, index) => (
                        <div>
                            <label>{item.type}</label>
                            <input type="checkbox"
                                   disabled={!this.props.isInitState}
                                   checked={item.active}
                                   onChange={({target}) => {
                                       let newAvailableTransportTypes = this.state.availableTransportTypes;
                                       newAvailableTransportTypes[index].active = !newAvailableTransportTypes[index].active
                                       this.setState({availableTransportTypes: newAvailableTransportTypes});
                                   }}/>
                        </div>
                    ))}
                    {this.props.isInitState
                        ? (<input className="btn btn-success" type="button"
                                  onClick={() => this.props.setMainPropertiesTransport(this.state)}
                                  value="Create"/>)
                        : (<input className="btn btn-info" type="button"
                                  onClick={() => this.props.setInitState()}
                                  value="Reset"/>)
                    }
                </form>
            </div>);

        let step = this.props.iterationResults[this.props.iterationResults.length - 1];
        let manyPassengersView = (!this.props.isInitState && this.props.isInteractiveMode)
            ? <SmoPassengersView smoLastStep={step} passengers={step.passengers} columns={this.state.columns}/> :
            <div/>;

        let runButton = (
            <button className="btn btn-success" onClick={() => this.props.runNextStepTransports()}>Run
                next step</button>);
        let runOneHundredStepsButton = (
            <button className="btn btn-success" onClick={() => this.props.runNextOneHundredStepsTransports()}>Run
                100 steps</button>);
        let buttonsForPassengersSystem = (!this.props.isInitState && this.props.iterationResults.length !== 0) ?
            (<div style={{"margin-bottom": "20px"}}>
                <div style={{display: "inline-block", "margin-right": "10px"}}>
                    {runButton}
                </div>
                <div style={{display: "inline-block", "margin-right": "10px"}}>
                {runOneHundredStepsButton}
                </div>
            </div>) : <div/>;

        let results = this.props.iterationResults.map(x => x.averageSatisfaction);
        let resultChart = ((!this.props.isInitState && this.props.iterationResults.length !== 0))
            ? <SmoResultChart data={results}/>
            : <div/>;

        let dataForCountChart = this.getDataFroCountChart(this.props.iterationResults);
        let countChart = ((!this.props.isInitState && this.props.iterationResults.length !== 0))
            ? <TransportsCountChart transports={dataForCountChart}/>
            : <div/>;

        let dataForQualityChart = this.getDataForQualityChart(this.props.iterationResults);
        let qualityChart = ((!this.props.isInitState && this.props.iterationResults.length !== 0))
            ? <TransportsQualityChart transports={dataForQualityChart}/>
            : <div/>;

        return (
            <div>
                {initialFrom}
                {manyPassengersView}
                {buttonsForPassengersSystem}
                {resultChart}
                {countChart}
                {qualityChart}
            </div>

        )
    }
}

export default connect(
    state => state.transports,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Transports);