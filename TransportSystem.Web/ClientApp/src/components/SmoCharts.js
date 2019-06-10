import React from 'react';
import Highcharts from 'highcharts';
import {
    HighchartsChart, Chart, withHighcharts, XAxis, YAxis, Title, Subtitle, Legend, ColumnSeries
} from 'react-jsx-highcharts';

class App extends React.Component {

    constructor(props) {
        super(props)
    }

    render() {
        let data = this.props.data;
        return (
            <div className="app">
                <HighchartsChart options={{
                    legend: {
                        enabled: false
                    }
                }}>
                    <Chart/>

                    <Title>Quality of transport</Title>
                    <Subtitle>red - terrible trips, yellow - normal trips, green - good trips</Subtitle>
                    <Legend layout="vertical" align="right" verticalAlign="middle"/>

                    <XAxis categories={data.keys}
                           plotBands={[
                               {
                                   from: data.minQualityValue,
                                   to: data.firstLine,
                                   color: 'rgba(232, 32, 32, .2)'
                               },   
                               {
                                   from: data.firstLine,
                                   to: data.secondLine,
                                   color: 'rgba(232, 199, 32, .2)'
                               },
                               {
                                   from: data.secondLine,
                                   to: data.maxQualityValue,
                                   color: 'rgba(58, 153, 49, .3)'
                               }]}>
                        <XAxis.Title>Quality of trips</XAxis.Title>
                    </XAxis>

                    <YAxis>
                        <YAxis.Title>Number of passengers</YAxis.Title>
                        <ColumnSeries name="Trips of bus" data={data.values}/>
                    </YAxis>
                </HighchartsChart>
            </div>
        );
    }
}

export default withHighcharts(App, Highcharts);