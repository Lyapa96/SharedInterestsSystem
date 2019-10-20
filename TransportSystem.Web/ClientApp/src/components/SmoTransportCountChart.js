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

const App = ({carData, busData, subwayData, bikeData, tramData}) => (
    <div className="app">
        <HighchartsChart plotOptions={plotOptions}>
            <Chart />

            <Title>Iterations results: count passengers on transports</Title>

            <Legend layout="vertical" align="right" verticalAlign="middle" />

            <XAxis>
                <XAxis.Title>Iterations</XAxis.Title>
            </XAxis>

            <YAxis>
                <YAxis.Title>Count passengers on transports</YAxis.Title>
                <LineSeries name="Car" data={carData} />
                <LineSeries name="Bus" data={busData} />
                <LineSeries name="Subway" data={subwayData} />
                <LineSeries name="Bike" data={bikeData} />
                <LineSeries name="Tram" data={tramData} />
            </YAxis>
        </HighchartsChart>
    </div>
);

export default withHighcharts(App, Highcharts);