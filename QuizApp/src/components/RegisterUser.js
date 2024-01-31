import { useState } from "react";
import './RegisterUser.css';
import axios from "axios";
import { Link, useNavigate } from "react-router-dom";

function RegisterUser(){
    const roles =["Creator","Participant"];
    const [username,setUsername] = useState("");
    const [password,setPassword] = useState("");
    const[email,setEmail]=useState("");
    const [repassword,setrePassword] = useState("");
    const [role,setRole] = useState("");
    var [usernameError,setUsernameError]=useState("");
    var [passwordError,setPasswordError]=useState("");
    var [rePasswordError,setRePasswordError]=useState("");
    var [roleError,setRoleError]=useState("");

    const navigate = useNavigate();

    var checkUSerData = ()=>{
        const usernameRegex=/^[a-z_][a-z0-9_]*$/;
        if(!usernameRegex.test(username))
        {
            setUsernameError("Invalid username format username cannot be empty shouldn't start with digit username should contain lowercase letters, digits and underscores, and not include uppercase letters.");
            return false;
        }
        else{
            setUsernameError("");
        }
        if(password.length<6){
            setPasswordError("Password should have atleast 6 characters");
            return false;
        }
        else{
            setPasswordError("");
        }
        if(repassword!==password){
            setRePasswordError("Repassword didn't match with password");
            return false;
        }
        else{
            setRePasswordError("");
        }
        if(role===''){
            setRoleError("Please select the role");
            return false;
        }
        else{
            setRoleError("");
        }
    }
    const signUp = (event)=>{
        event.preventDefault();
        var checkData = checkUSerData();
        if(checkData===false)
        {
            alert('please check your data')
            return;
        }
        
        axios.post("http://localhost:5252/api/User/register",{
            username: username,
            role:	role,
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
            if(err.response.data==="Duplicate username"){
                alert('The username already exists, please login if that is urs');
            }
            console.log(err)
        })
    }
    
    return(
        <form className="registerForm">
            <div class="icon">
                <img src="./icon.jpg" alt="QuizCraft"/>
            </div>
            <h1 class="text-center mt-4 name">Register</h1>
            <div class="form-floating mb-3">
            <input type="text" class="form-control" id="floatingInput" placeholder="Username" value={username} onChange={(e)=>{setUsername(e.target.value)}}/>
            <label for="floatingInput">Username</label>
        </div>
            <label className="alert alert-danger">{usernameError}</label>
            <div class="form-floating mb-3">
            <input type="email" class="form-control" id="floatingPassword" placeholder="Email"
            value={email} onChange={(e)=>{setEmail(e.target.value)}}/>
            <label for="floatingInput">Email</label>
        </div>
        <div class="form-floating">
            <input type="password" class="form-control" id="floatingPassword" placeholder="Password"
            value={password} onChange={(e)=>{setPassword(e.target.value)}}/>
            <label for="floatingPassword">Password</label>
        </div>
            <label className="alert alert-danger">{passwordError}</label><br/>
        <div class="form-floating">
            <input type="Password" class="form-control" id="floatingPassword" placeholder="ReTypePassword"
            value={repassword} onChange={(e)=>{setrePassword(e.target.value)}}/>
            <label for="floatingPassword">ReTypePassword</label>
        </div>
        <label className="alert alert-danger">{rePasswordError}</label><br/>
            <select className="form-select" onChange={(e) => { setRole(e.target.value) }}>
                <option value="select">Select Role</option>
                {roles.map((r) =>
                    <option value={r} key={r}>{r}</option>
                )}
            </select>
            <label className="alert alert-danger">{roleError}</label><br/>
            <br/>
            <button className="btn btn-login button" onClick={signUp}>Sign Up</button>
            
            <hr/>
            <div class="text-center fs-6">
            Already have an account? <Link to="/login" data-bs-toggle="tooltip" title="Login!">Login</Link>
            </div>
            
        </form>
    );
}

export default RegisterUser;