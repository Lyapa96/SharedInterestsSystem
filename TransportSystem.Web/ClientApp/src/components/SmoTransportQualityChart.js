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

const App = ({carQualityData, busQualityData, subwayQualityData, bikeQualityData, tramQualityData}) => (
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
                <LineSeries name="Car" data={carQualityData} />
                <LineSeries name="Bus" data={busQualityData} />
                <LineSeries name="Subway" data={subwayQualityData} />
                <LineSeries name="Bike" data={bikeQualityData} />
                <LineSeries name="Tram" data={tramQualityData} />
            </YAxis>
        </HighchartsChart>
    </div>
);

export default withHighcharts(App, Highcharts);