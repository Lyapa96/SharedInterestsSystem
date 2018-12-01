import React from 'react';
import { connect } from 'react-redux';

class PassengersShow extends React.Component {
    constructor(props){
        super(props)
    }
    
    render() {
        let passengers = this.props.passengers;
        let array = [];
        
        for (let i = 0; i < this.props.height; i++) {
            for (let j = 0; j < this.props.width; j++) {
                let passenger = passengers[i][j];
                array.push((<div>

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
                            ? <img className="automobile" src="../../public/favicon.ico" alt="car"/>
                            : <img className="automobile" src="favicon.ico" alt="bus"/>}
                    </div>
                </div>));
            }

        }

        return (<div>{array}</div>);
    }
}

export default connect()(PassengersShow);
