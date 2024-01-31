import "./Quiz.css";
function Quiz()
{
    var quiz={
        title:"fun with maths",
        description:"This quiz is just to have fun with maths",
        category:"Maths"
    }
    return(
        <div className="quiz">
            <div>
                Quiz Title:{quiz.title}
                <br/>
                Quiz Description:{quiz.description}
                <br/>
                Quiz Category:{quiz.category}
            </div>
        </div>
    )
}
export default Quiz;