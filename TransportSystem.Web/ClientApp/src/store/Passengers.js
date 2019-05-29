const setInitStateType = 'SET_INIT_STATE';
const setMainProperties = 'SET_MAIN_PROPERTIES';
const setInteractiveMode = 'SET_INTERACTIVE_MODE';
const getNextStep = 'NEXT_STEP';

const initialState = {initState: true, columns: 0, rows: 0, algorithm: null, smoStep: null, interactiveMode: false};

export const actionCreators = {
    setInitState: () => ({type: setInitStateType}),
    setMainProperties: (data) => {
        return ({type: setMainProperties, payload: data})
    },
    setInteractiveMode: (data) => async (dispatch, getState) => {
        //console.log(data.passengers[1].id);
        let body = JSON.stringify({columns: data.columns, passengers: data.passengers});
        console.log(body);
        const url = 'https://localhost:5003/api/passengers/set';
        const response = await fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: body
        });
        const newSmoStep = await response.json();
        console.log("cool");
        console.log(newSmoStep);
        //console.log(newPassengers);
        dispatch({type: setInteractiveMode, payload: newSmoStep})
    },
    getNextStep: () => async (dispatch, getState) => {
        let state = getState().passengers;
        console.log(state);
        //const url = 'https://transportsysteminfra.azurewebsites.net/api/passengers/smoStep';
        const url = 'https://localhost:5003/api/passengers/smoStep';
        const response = await fetch(url, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify(state.smoStep)
        });
        const newSmoStep = await response.json();

        dispatch({type: getNextStep, payload: newSmoStep});
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
        return {...state, smoStep: action.payload, interactiveMode: true}
    }

    if (action.type === getNextStep) {
        return {...state, smoStep: action.payload}
    }

    return state;
};

