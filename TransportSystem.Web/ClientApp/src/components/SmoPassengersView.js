import React from 'react';
import './PassengersShow.css';

export default ({smoLastStep, columns}) => {
        let { smoPassengerInfo, iterationStep, averageSatisfaction} = smoLastStep;
        let passengers = smoPassengerInfo;
        let rows = [];
        let row = [];
        for (let item of passengers) {
            if (row.length === columns) {
                rows.push((<div className="passengersRow">{row}</div>));
                row = [];
            }
            let classes = `small-cell ${item.type === 'Car' ? 'small-cell-shadow-red' : 'small-cell-shadow-blue'}`;
            let image = `${item.type === 'Car' ? 'car-svg.svg' : 'bus-svg.svg'}`;
            row.push((<div className={classes}>
                <img src={image} width="18" height="18" alt="image format svg" />
                <p style={{margin: 0, "font-size": "9px"}}>ID={item.id}</p>
                <p style={{margin: 0, "font-size": "9px"}}>Q={item.quality}</p>
                <p style={{margin: 0, "font-size": "9px"}}>S={item.satisfaction}</p>
            </div>));
        }
        if (row.length !== 0)
            rows.push((<div className="passengersRow">{row}</div>));
        
        let averageQuality = passengers.reduce((sum, item) => {
            return sum + item.quality;
        }, 0)/passengers.length;
        return <div>
            <h3 style={{"text-align": "center"}}>Passengers system</h3>
            <h4 style={{"text-align": "center"}}>Average satisfaction: {averageSatisfaction}</h4>
            <h4 style={{"text-align": "center"}}>Iteration: {iterationStep}</h4>
            {rows}
        </div>
}