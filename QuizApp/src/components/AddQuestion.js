import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "./AddQuestion.css";
function AddQuestion(){
    const [questionTxt,setQuestionTxt] = useState("");
    const [option1,setOption1] = useState("");
    const [option2,setOption2] = useState("");
    const [option3,setOption3]=useState("");
    const [option4,setOption4]= useState("");
    const [answer,setAnswer] = useState("");
    const[quizId,setQuizId]= useState(0);
    const navigate = useNavigate();
    const token=localStorage.getItem("token");
    var question;
    var clickAdd = ()=>{
       question={
        "questionTxt":questionTxt,
        "option1":option1,
        "option2":option2,
        "option3":option3,
        "option4":option4,
        "answer":answer,
        "quizId":quizId
        }
        console.log(question);
        fetch('http://localhost:5252/api/Questions/add',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                Authorization: `Bearer ${token}`,
                'Content-Type':'application/json'
            },
            body:JSON.stringify(question)
        }).then(
            ()=>{
                alert("Question Added Successfully");
                navigate("/questions");
            }
        ).catch((e)=>{
            console.log(e)
        })
    }


    return(
        <div className="inputcontainer">
            <h2 className="alert alert-quiz">Add Questions</h2>
            <div className="form-floating mb-3">
                <input id="floatingInput" type="text" className="form-control" value={questionTxt} placeholder="Question" onChange={(e)=>{setQuestionTxt(e.target.value)}}/>
                <label htmlFor="floatingInput">Question</label>
            </div>
            <div className="form-floating mb-3">
                <input id="floatingInput" type="text" className="form-control" value={option1} placeholder="Option A" onChange={(e)=>{setOption1(e.target.value)}}/>
                <label htmlFor="floatingInput">Option A</label>
            </div>
            <div className="form-floating mb-3">
                <input id="floatingInput" type="text" className="form-control" value={option2} placeholder="Option B" onChange={(e)=>{setOption2(e.target.value)}}/>
                <label htmlFor="floatingInput">Option B</label>
            </div>
            <div className="form-floating mb-3">
                <input id="floatingInput" type="text" className="form-control" value={option3} placeholder="Option C" onChange={(e)=>{setOption3(e.target.value)}}/>
                <label htmlFor="floatingInput">Option C</label>
            </div>
            <div className="form-floating mb-3">
                <input id="floatingInput" type="text" className="form-control" value={option4} placeholder="Option D" onChange={(e)=>{setOption4(e.target.value)}}/>
                <label htmlFor="floatingInput">Option D</label>
            </div>
            <div className="form-floating mb-3">
                <input id="floatingInput" type="text" className="form-control" value={answer} placeholder="Answer" onChange={(e)=>{setAnswer(e.target.value)}}/>
                <label htmlFor="floatingInput">Answer</label>
            </div>
            <div className="form-floating mb-3">
                <input id="floatingInput" type="number" className="form-control" value={quizId} placeholder="Quiz Id" onChange={(e)=>{setQuizId(e.target.value)}}/>
                <label htmlFor="floatingInput">Quiz Id</label>
            </div>
            <button onClick={clickAdd} className="btn btn-primary">Add Question</button>
        </div>
    );
}

export default AddQuestion;