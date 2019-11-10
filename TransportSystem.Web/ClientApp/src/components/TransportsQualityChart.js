import React, { Component } from 'react';
import Highcharts from 'highcharts';
import {
    HighchartsChart, Chart, withHighcharts, XAxis, YAxis, Title, Subtitle, Legend, LineSeries
} from 'react-jsx-highcharts';

const plotOptions = {
    series: {
        pointStart: 0
    }
};

const App = ({transports}) => (
    <div className="app">
        <HighchartsChart plotOptions={plotOptions}>
            <Chart />

            <Title>Iterations results: average satisfaction passengers by transport</Title>

            <Legend layout="vertical" align="right" verticalAlign="middle" />

            <XAxis>
                <XAxis.Title>Iterations</XAxis.Title>
            </XAxis>

            <YAxis>
                <YAxis.Title>Average satisfaction passengers by transport</YAxis.Title>
                {transports.map(x => (<LineSeries name={x.name} data={x.iterationsData} />))}
            </YAxis>
        </HighchartsChart>
    </div>
);

export default withHighcharts(App, Highcharts);