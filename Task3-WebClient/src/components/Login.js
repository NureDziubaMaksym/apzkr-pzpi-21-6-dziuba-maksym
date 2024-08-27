import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';

function Login() {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch('http://localhost:5275/auth/Auth/login-admin', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ login, password })
            });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('token', data.token);
                localStorage.setItem('login', login);

                // Дополнительный запрос для получения данных пользователя по логину
                const userResponse = await fetch(`http://localhost:5275/users/User/bylogin/${login}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${data.token}` // Используем токен для авторизации
                    }
                });

                if (userResponse.ok) {
                    const userData = await userResponse.json();
                    // Сохраняем дополнительные данные в localStorage
                    localStorage.setItem('email', userData.email);
                    localStorage.setItem('phoneNumber', userData.phoneNumber);
                    localStorage.setItem('name', userData.name);

                    // Перенаправляем на основную страницу
                    navigate('/main');
                } else {
                    alert('Failed to fetch user data');
                }
            } else {
                alert('Login failed');
            }
        } catch (error) {
            console.error('Error during login:', error);
            alert('An error occurred. Please try again later.');
        }
    };

    return (
        <div className="login-container">
            <form onSubmit={handleSubmit} className="login-form">
                <h2>LOGIN FORM</h2>
                <div className="input-group">
                    <input 
                        type="text" 
                        placeholder="Username" 
                        value={login} 
                        onChange={(e) => setLogin(e.target.value)} 
                    />
                </div>
                <div className="input-group">
                    <input 
                        type="password" 
                        placeholder="Password" 
                        value={password} 
                        onChange={(e) => setPassword(e.target.value)} 
                    />
                </div>
                <button type="submit" className="login-button">Login</button>
                <p className="register-field">
                    <a href="#" onClick={() => navigate('/register')}>Do not have an account? <span>Register</span></a>
                </p>
            </form>
        </div>
    );
}

export default Login;
