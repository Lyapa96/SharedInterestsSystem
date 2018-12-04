import React from 'react';
import { connect } from 'react-redux';
import './PassengersShow.css';

class PassengersShow extends React.Component {
    constructor(props){
        super(props)
    }
    
    render() {
        let passengers = this.props.passengers;
        let allPassengersCells = [];
        
        for (let i = 0; i < this.props.height; i++) {
            let rows = [];
            for (let j = 0; j < this.props.width; j++) {
                let passenger = passengers[i][j];
                rows.push((
                    <div className="cell">
                        <h4>
                            Passenger {passenger.number}
                        </h4>
                        <p>
                            Satisfaction: <strong>{passenger.satisfaction}</strong>
                            <br/>
                            Quality of services: <strong>{passenger.quality}</strong>
                        </p>
                        {(passenger.transportType === "car")
                            ? <img className="automobile" src="car.jpg" alt="car"/>
                            : <img className="automobile" src="bus.png" alt="bus"/>}
                    </div>));
            }
            allPassengersCells.push(<div className="passengersRow">{rows}</div>);
        }

        return (<div>{allPassengersCells}</div>);
    }
}

export default connect()(PassengersShow);
