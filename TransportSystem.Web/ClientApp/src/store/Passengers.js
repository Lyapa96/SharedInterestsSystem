const setInitStateType = 'SET_INIT_STATE';
const setMainProperties = 'SET_MAIN_PROPERTIES';
const setInteractiveMode = 'SET_INTERACTIVE_MODE';
const getNextStep = 'NEXT_STEP';
const updatePassengerAction = 'UPDATE_PASSENGER';

const initialState = {initState: true, columns: 0, rows: 0, algorithm: null, smoStep: null, interactiveMode: false};
//const url = 'https://transportsysteminfra.azurewebsites.net/api/passengers/';
const url = `https://localhost:5003/api/passengers/`;

export const actionCreators = {
    setInitState: () => ({type: setInitStateType}),
    setMainProperties: (data) => async (dispatch) => {
        let columns = Number(data.columns);
        let rows = Number(data.rows);
        let body = JSON.stringify({columns: columns, rows: rows, algorithmType: data.algorithmType});
        const method = 'init';
        const response = await fetch(url + method, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: body
        });
        const iterationResult = await response.json();
        dispatch({
            type: setMainProperties,
            payload: {
                iterationResult: iterationResult,
                columns: columns,
                rows: rows,
                algorithmType: data.algorithmType
            }
        });
    },
    updatePassenger: (data) => ({type: updatePassengerAction, payload: data}),
    setInteractiveMode: () => ({type: setInteractiveMode}),
    getNextStep: () => async (dispatch, getState) => {
        let state = getState().passengers;
        const method = 'nextStep';
        const response = await fetch(url + method, {
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

    if (action.type === setInitStateType) {
        return initialState;
    }

    if (action.type === setMainProperties) {
        let myState = {
            ...state,
            initState: false,
            columns: action.payload.columns,
            rows: action.payload.rows,
            algorithm: action.payload.algorithmType,
            smoStep: action.payload.iterationResult
        };

        return myState;
    }

    if (action.type === updatePassengerAction) {
        let newSmpStep = {
            ...state.smoStep
        };
        newSmpStep.passengers[action.payload.passengerIndex] = action.payload.newParameters;

        return {
            ...state,
            smoStep: newSmpStep
        }
    }

    if (action.type === setInteractiveMode) {
        return {...state, interactiveMode: true}
    }

    if (action.type === getNextStep) {
        return {...state, smoStep: action.payload}
    }

    return state;
};

