const setInitStateType = 'SET_INIT_STATE';
const setMainProperties = 'SET_MAIN_PROPERTIES';
const setInteractiveMode = 'SET_INTERACTIVE_MODE';
const getNextStep = 'NEXT_STEP';

const initialState = { initState: true, columns: 0, rows: 0, algorithm: null, passengers: null, interactiveMode: false };

export const actionCreators = {
    setInitState: () => ({ type: setInitStateType }),
    setMainProperties: (data) => {return ({ type: setMainProperties, payload: data })},
    setInteractiveMode: (data) => {return ({ type: setInteractiveMode, payload: data})},
    getNextStep: () => async (dispatch, getState) => {
        let state = getState().passengers;
        console.log(state);
        const url = 'https://transportsysteminfra.azurewebsites.net/api/passengers';
        const response = await fetch(url,{
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify({algorithmType: state.algorithm.type, passengers: state.passengers})
        });
        const newPassengers = await response.json();

        dispatch({type: getNextStep, payload: newPassengers});
    }
};

export const reducer = (state, action) => {
    state = state || initialState;
    console.log(`Passengers log input action: ${action.type}`);
    console.log(state);
    if (action.type === setInitStateType) {
        return initialState;
    }

    if (action.type === setMainProperties) {
        let myState = {
            ...state,
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
        return { ...state, passengers: action.payload.passengers}
    }

    return state;
};

