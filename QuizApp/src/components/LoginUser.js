import axios from "axios";
import { useState } from "react";
import { Link,useNavigate } from "react-router-dom";
import "./Login.css";
function LoginUser(){
    const [username,setUsername] = useState("");
    const [password,setPassword] = useState("");
    const navigate = useNavigate();
    var [usernameError,setUsernameError]=useState("");
    var [passwordError,setPasswordError]=useState("");
    var checkUSerData = ()=>{
        if(username==='')
        {
            setUsernameError("Username cannot be empty");
            return false;
        }
        else{
            setUsernameError("");
        }
           
        if(password===''){
            setPasswordError("Password cannot be empty");
            return false;
        }
        else{
            setPasswordError("");
        }
    }
    const Login = (event)=>{
        event.preventDefault();
        var checkData = checkUSerData();
        if(checkData===false)
        {
            alert('please check your data')
            return;
        }
        
        axios.post("http://localhost:5252/api/User/login",{
            username: username,
            password:password
        })
        .then((userData)=>{
            var token = userData.data.token;
            localStorage.setItem("token",token);
            var username=userData.data.username;
            localStorage.setItem("username",username);
            var role=userData.data.role;
            localStorage.setItem("role",role);
            alert('Welcome to the quizapp :'+username);
            navigate("/")
            
        })
        .catch((err)=>{
            if(err.response.data==="Invalid username or password"){
                alert('Either username or password does not match');
            }
            console.log(err)
        })
    }
    
    return(
        <div class="wrapper">
            <div>
        <form className="loginForm">
        <div class="icon">
            <img src="./icon.jpg" alt="QuizCraft"/>
        </div>
        <h1 class="text-center mt-4 name">
            Login
        </h1>
        <div class="form-floating mb-3">
            <input type="text" className="form-control" id="floatingInput" placeholder="Username" value={username} onChange={(e)=>{setUsername(e.target.value)}}/>
            <label htmlFor="floatingInput">Username</label>
        </div>
            <label className="alert alert-danger">{usernameError}</label>
        <div class="form-floating">
            <input type="password" className="form-control" id="floatingPassword" placeholder="Password"
            value={password} onChange={(e)=>{setPassword(e.target.value)}}/>
            <label htmlFor="floatingPassword">Password</label>
        </div>
            <label className="alert alert-danger">{passwordError}</label><br/>
            <button className="btn btn-login button" onClick={Login}>Login</button>
        </form>
            <div class="text-center fs-6">
            New User? <Link to="/register" data-bs-toggle="tooltip" title="Register!">Register</Link>
            </div>
        </div>
        </div>
        
    );
}

export default LoginUser;