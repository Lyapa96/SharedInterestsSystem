import React from 'react';
import './PassengersShow.css';

export default ({smoLastStep, columns}) => {
    let {passengers, iterationStep, averageSatisfaction} = smoLastStep;

    let rows = [];
    let row = [];

    function getImage(transportType) {
        switch (transportType) {
            case 'Car':
                return 'car-svg.svg';
            case 'Subway':
                return 'subway-svg.svg';
            case 'Bus':
                return 'bus-svg.svg';
            case 'Bike':
                return 'bike-svg.svg';
            case 'Tram':
                return 'tram-svg.svg';
        }
        return '';
    }

    for (let item of passengers) {
        if (row.length === columns) {
            rows.push((<div className="passengersRow">{row}</div>));
            row = [];
        }
        let classes = `small-cell ${item.transportType === 'Car' ? 'small-cell-shadow-red' : 'small-cell-shadow-blue'}`;
        let image = `${(getImage(item.transportType))}`;
        row.push((<div className={classes}>
            <img src={image} width="18" height="18" alt="image format svg"/>
            <p style={{margin: 0, "font-size": "9px"}}>ID={item.id}</p>
            <p style={{margin: 0, "font-size": "9px"}}>Q={item.quality}</p>
            <p style={{margin: 0, "font-size": "9px"}}>S={item.satisfaction}</p>
        </div>));
    }
    if (row.length !== 0)
        rows.push((<div className="passengersRow">{row}</div>));

    return <div>
        <h3 style={{"text-align": "center"}}>Passengers system</h3>
        <h4 style={{"text-align": "center"}}>Average satisfaction: {averageSatisfaction}</h4>
        <h4 style={{"text-align": "center"}}>Iteration: {iterationStep}</h4>
        {rows}
    </div>
}