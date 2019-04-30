import React from 'react';
import {connect} from 'react-redux';
import SmoCharts from './SmoCharts';
import {actionCreators} from '../store/Smo';
import {bindActionCreators} from "redux";
import SmoTable from './SmoTable';
import {getDataToChart} from '../helpers/passengresAggregator';

import {toCsvContent} from "../helpers/csvSelrializer";
import {downloadCsv} from "../helpers/downloadCsv";
import SmoPassengersView from "./SmoPassengersView";
import SmoResultChart from "./SmoResultChart";

const visualisationOptions = [
    {
        columnName: 'Agent Id',
        objectFieldName: 'agentId'
    },
    {
        columnName: 'Agent arrive time',
        objectFieldName: 'arriveAgentTime'
    },
    {
        columnName: 'Queue Count',
        objectFieldName: 'queueCount'
    },
    {
        columnName: 'Agents Count',
        objectFieldName: 'agentsCount'
    },
    {
        columnName: 'Quality',
        objectFieldName: 'quality'
    }
];

class Smo extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isInitState: true,
            isInteractiveMode: false,
            showSmoResult: false,
            passengersOnCar: 15,
            passengersOnBus: 35,
            columns: 10,
            channelCount: 10,
            neighboursCount: 7
        }
    }

    render() {
        let mainPart = (
            <div>
                <h1>Queueing model</h1>
                <button className="btn btn-success" onClick={this.props.setInitState}>Reset</button>
            </div>);
        let initialFrom = (
            <div>
                <h3>Init parameters</h3>
                <form>
                    <p>
                        Number of passengers on bus: <input type="number" name="columnCount" onChange={({target}) => {
                        this.setState({passengersOnBus: Number(target.value)})
                    }} value={this.state.passengersOnBus} step="1" min="1" max="50"/>
                    </p>
                    <p>
                        Number of passengers on car: <input type="number" name="columnCount" onChange={({target}) => {
                        this.setState({passengersOnCar: target.value})
                    }} value={this.state.passengersOnCar} step="1" min="1" max="50"/>
                    </p>
                    <p>
                        Number of channel: <input type="number" name="columnCount" onChange={({target}) => {
                        this.setState({channelCount: Number(target.value)})
                        }} value={this.state.channelCount} step="1" min="1" max="20"/>
                    </p>
                    <p>
                        Columns: <input type="number" name="columnCount" onChange={({target}) => {
                        this.setState({columns: target.value})
                    }} value={this.state.columns} step="1" min="1" max="10"/>
                    </p>
                    <p>
                        Number of neighbours: <input type="number" name="columnCount" onChange={({target}) => {
                        this.setState({neighboursCount: target.value})
                    }} value={this.state.neighboursCount} step="1" min="2" max="10"/>
                    </p>
                    <input className="btn btn-success" type="button"
                           onClick={() => this.props.getDataFromSockets(this.state)}
                           value="Create"/>
                </form>
            </div>);

        let chart = (!this.props.isInitState) ?
            <SmoCharts data={getDataToChart(this.props.smoResults, true)}/> : <div/>;

        let showSmoResultButton = (!this.props.isInitState)
            ? <button className="btn btn-success" onClick={() => this.setState(state => {
                state.showSmoResult = !state.showSmoResult;
                return state
            })}>
                {this.state.showSmoResult ? "Hide statistics" : "Show detailed statistics"}</button>
            : <div/>;
        let table = (!this.props.isInitState && this.state.showSmoResult)
            ? (<SmoTable data={this.props.smoResults}/>)
            : <div/>;

        let downloadSmoButton = (!this.props.isInitState)
            ? <button className="btn btn-primary" href="https://localhost:5003/api/passengers" onClick={() => {
                console.log("download csv");
                let headers = visualisationOptions.map(x => x.columnName);
                let data = this.props.smoResults.map(x => visualisationOptions.map(y => x[y.objectFieldName].toString()));
                let content = toCsvContent(headers, data);
                downloadCsv(content, 'data');
            }
            }>Download csv</button>
            : <div/>;

        let setInteractiveModeButton = (!this.props.isInitState && this.props.smoSteps.length === 0)
            ? <button className="btn btn-success" onClick={() => this.props.setSmoInteractiveMode(this.state)}>Start interactive
                mod</button>
            : <div/>;

        let manyPassengersView = (!this.props.isInitState && this.props.isInteractiveMode) 
            ? <SmoPassengersView smoLastStep={this.props.smoSteps[this.props.smoSteps.length-1]} columns={this.state.columns}/> :
            <div/>;

        let runButton = (<button className="btn btn-success" onClick={() => this.props.runNextStep()}>Run next step</button>);
        let downloadResultButton = (<button className="btn btn-primary" href="https://localhost:5003/api/passengers" onClick={() => {
            console.log("download csv");
            let headers = visualisationOptions.map(x => x.columnName);
            let data = this.props.smoResults.map(x => visualisationOptions.map(y => x[y.objectFieldName].toString()));
            let content = toCsvContent(headers, data);
            downloadCsv(content, 'data');
        }
        }>Download csv</button>);
        let buttonsForPassengersSystem = (!this.props.isInitState && this.props.smoSteps.length !== 0) ?
            (<div style={{"margin-bottom": "20px"}}>
                <div style={{display: "inline-block", "margin-right": "10px"}}>
                {runButton}
                </div>
                {downloadResultButton}
            </div>) : <div/>;
        
        let results = this.props.smoSteps.map(x => x.averageSatisfaction);
        let restulChart = ((!this.props.isInitState && this.props.smoSteps.length !== 0))
        ? <SmoResultChart data={results}/>
        : <div/>;
        
        return (
            <div>
                {mainPart}
                {initialFrom}
                {chart}
                <div style={{"margin-bottom": "20px"}}>
                    <div style={{display: "inline-block", "margin-right": "10px"}}>
                        {showSmoResultButton}
                    </div>
                    {downloadSmoButton}
                </div>
                {table}
                {setInteractiveModeButton}
                {manyPassengersView}
                {buttonsForPassengersSystem}
                {restulChart}
            </div>)
    }
}

export default connect(
    state => state.smo,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(Smo);