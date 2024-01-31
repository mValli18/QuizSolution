import {createSlice} from '@reduxjs/toolkit';


const quizSlice = createSlice({
    name:"quiz",
    initialState:[],
    reducers:{
        addItem:(state,action)=>{
            state.push(action.payload);
        }
    }
})

export const{addItem} = quizSlice.actions;

export default quizSlice.reducer;