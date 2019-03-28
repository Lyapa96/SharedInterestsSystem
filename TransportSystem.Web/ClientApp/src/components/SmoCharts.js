import React, {Component} from 'react';
import Highcharts from 'highcharts';
import {
    HighchartsChart, Chart, withHighcharts, XAxis, YAxis, Title, Subtitle, Legend, ColumnSeries
} from 'react-jsx-highcharts';


const plotOptions = {
    series: {},
    column: {}
};

const App = () => (
    <div className="app">
        <HighchartsChart plotOptions={plotOptions}>
            <Chart/>

            <Title>Quality of transport</Title>
            <Subtitle>red - terrible trips, yellow - normal trips, green - good trips</Subtitle>
            <Legend layout="vertical" align="right" verticalAlign="middle"/>

            <XAxis categories={['1', '3', '7', '10', '11', '12', '20', '25']}
                   plotBands={[
                       {
                           from: -1,
                           to: 2.5,
                           color: 'rgba(232, 32, 32, .2)'
                       },
                       {
                           from: 2.5,
                           to: 5.5,
                           color: 'rgba(232, 199, 32, .2)'
                       },
                       {
                           from: 5.5,
                           to: 7.5,
                           color: 'rgba(58, 153, 49, .3)'
                       }]}>
                <XAxis.Title>Quality of trips</XAxis.Title>
            </XAxis>

            <YAxis>
                <YAxis.Title>Number of passengers</YAxis.Title>
                <ColumnSeries name="Trips of bus" data={[14, 9, 7, 10, 11, 12, 6, 3]}/>
            </YAxis>
        </HighchartsChart>
    </div>
);

export default withHighcharts(App, Highcharts);