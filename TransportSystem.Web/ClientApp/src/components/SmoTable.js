import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';


export default class SmoTable extends React.Component {
    constructor(props){
        super(props);
    }
    
    render() {
        return (
            <BootstrapTable data={ this.props.data } striped={ true } hover={ true } condensed={ true }>
                <TableHeaderColumn isKey dataField='agentId'>Agent ID</TableHeaderColumn>
                <TableHeaderColumn dataField='arriveAgentTime'>Agent arrive time</TableHeaderColumn>
                {/*<TableHeaderColumn dataField='startTime'>Start Time</TableHeaderColumn>*/}
                {/*<TableHeaderColumn dataField='endTime'>End Time</TableHeaderColumn>*/}
                <TableHeaderColumn dataField='queueCount'>Queue Count</TableHeaderColumn>
                <TableHeaderColumn dataField='agentsCount'>Agents Count</TableHeaderColumn>
                {/*<TableHeaderColumn dataField='channelNumber'>Channel Number</TableHeaderColumn>*/}
                {/*<TableHeaderColumn dataField='edgeNumber'>Edge Number</TableHeaderColumn>*/}
                <TableHeaderColumn dataField='quality'>Quality</TableHeaderColumn>
            </BootstrapTable>
        );
    }
}