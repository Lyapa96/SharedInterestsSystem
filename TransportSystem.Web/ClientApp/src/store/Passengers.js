const setInitStateType = 'SET_INIT_STATE';
const setMainProperties = 'SET_MAIN_PROPERTIES';
const setInteractiveMode = 'SET_INTERACTIVE_MODE';
const getNextStep = 'NEXT_STEP';

const initialState = { initState: true, columns: 0, rows: 0, algorithm: null, passengers: null, interactiveMode: false };



function fetchDataFromApi(state) {
    console.log(state.algorithm.type);
    console.log(state.passengers);
    console.log(JSON.stringify({algorithmType: state.algorithm.type, passengers: state.passengers}));
    const url = 'https://localhost:5003/api/passengers';
    fetch(url,
        {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({algorithmType: state.algorithm.type, passengers: state.passengers})
        })
        .then(function(res){ console.log('невероятно');console.log(res) });
    
    return createRandomPassengers()
}

function createRandomPassengers() {
    let passengers = [];
    for (let i = 0; i < 3; i++) {
        let currentRow = [];
        for(let j = 0; j < 3; j++) {
            currentRow.push({
                number: `${i} ${j}`,
                satisfaction: Math.random().toFixed(2),
                quality: Math.random().toFixed(2),
                transportType: (Math.random() > 0.5)? "car":"bus"
            });
        }
        passengers.push(currentRow)
    }

    return passengers;
}


export const actionCreators = {
    setInitState: () => ({ type: setInitStateType }),
    setMainProperties: (data) => {return ({ type: setMainProperties, payload: data })},
    setInteractiveMode: (data) => {return ({ type: setInteractiveMode, payload: data})},
    getNextStep: () => async (dispatch, getState) => {
        let state = getState().passengers;
        console.log(state);
        const url = 'https://localhost:5003/api/passengers';
        const response = await fetch(url,{
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({algorithmType: state.algorithm.type, passengers: state.passengers})
        });
        const newPassengers = await response.json();
        console.log('aaaaaaaaaaaaaaaaaa');
        console.log(newPassengers);
        dispatch({type: getNextStep, payload: newPassengers});
    }
};

export const reducer = (state, action) => {
    state = state || initialState;
    console.log(action);
    if (action.type === setInitStateType) {
        return initialState;
    }

    if (action.type === setMainProperties) {
        console.log('====================');
        console.log(action.payload);
        let myState = { ...state, 
            initState: false, 
            columns: action.payload.columns, 
            rows: action.payload.rows, 
            algorithm: action.payload.algorithmType 
        };
        
        console.log(myState);
        return myState;
    }
    
    if (action.type === setInteractiveMode) {
        console.log(state);
        return { ...state, passengers: action.payload.passengers, interactiveMode: true}
    }
    
    if (action.type === getNextStep) {
        console.log('newnewnenwnenwnenenwnwnewnewnewnewn');
        console.log(action.payload);
        return { ...state, passengers: action.payload.passengers}
    }
    

    return state;
};

