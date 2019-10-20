import React from 'react';
import {connect} from 'react-redux';
import './PassengersShow.css';

class PassengersShow extends React.Component {
    constructor(props) {
        super(props)
    }

    render() {
        let {passengers, columns} = this.props;
        let rows = [];
        let row = [];
        for (let passenger of passengers) {
            if (row.length === columns) {
                rows.push((<div className="passengersRow">{row}</div>));
                row = [];
            }
            row.push((
                <div className="cell">
                    <h4>
                        Passenger {passenger.id}
                    </h4>
                    <p>
                        Satisfaction: <strong>{passenger.satisfaction}</strong>
                        <br/>
                        Quality of services: <strong>{passenger.quality}</strong>
                    </p>
                    {this.getImageForType(passenger.transportType)}
                </div>));
        }
        if (row.length !== 0)
            rows.push((<div className="passengersRow">{row}</div>));

        return (<div>{rows}</div>);
    }

    getImageForType(transportType) {
        switch (transportType) {
            case "Car":
                return <img className="automobile" src="car.jpg" alt="car"/>;
            case "Bus":
                return <img className="automobile" src="bus.png" alt="bus"/>;
        }

        return null;
    }
}

export default connect()(PassengersShow);
