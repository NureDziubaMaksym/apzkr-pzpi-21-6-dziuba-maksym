import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Register.css';

function Register() {
    const [step, setStep] = useState(1);
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [registerData, setRegisterData] = useState({
        name: '',
        email: '',
        phoneNumber: '',
        role: 'admin',
        age: '',
        race: '',
        gender: ''
    });
    const navigate = useNavigate();

    const handleNextStep = () => {
        if (password !== confirmPassword) {
            alert('Passwords do not match');
            return;
        }
        setStep(2);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('http://localhost:5275/auth/Auth/register-admin', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    login,
                    password,
                    ...registerData
                })
            });

            if (response.ok) {
                alert('Registration successful');
                navigate('/login');
            } else {
                alert('Registration failed');
            }
        } catch (error) {
            console.error('Error during registration:', error);
            alert('An error occurred. Please try again later.');
        }
    };

    const handleInputChange = (e, field) => {
        setRegisterData({ ...registerData, [field]: e.target.value });
    };

    return (
        <div className="register-container">
            <form onSubmit={step === 1 ? handleNextStep : handleSubmit} className="register-form">
                <h2>{step === 1 ? 'Set Login & Password' : 'Complete Your Profile'}</h2>
                {step === 1 ? (
                    <>
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
                        <div className="input-group">
                            <input 
                                type="password" 
                                placeholder="Confirm Password" 
                                value={confirmPassword} 
                                onChange={(e) => setConfirmPassword(e.target.value)} 
                            />
                        </div>
                        <button type="submit" className="register-button">Next</button>
                    </>
                ) : (
                    <>
                        <div className="input-group">
                            <input 
                                type="text" 
                                placeholder="Name" 
                                value={registerData.name} 
                                onChange={(e) => handleInputChange(e, 'name')} 
                            />
                        </div>
                        <div className="input-group">
                            <input 
                                type="email" 
                                placeholder="Email" 
                                value={registerData.email} 
                                onChange={(e) => handleInputChange(e, 'email')} 
                            />
                        </div>
                        <div className="input-group">
                            <input 
                                type="text" 
                                placeholder="Phone Number" 
                                value={registerData.phoneNumber} 
                                onChange={(e) => handleInputChange(e, 'phoneNumber')} 
                            />
                        </div>
                        <div className="input-group">
                            <input 
                                type="number" 
                                placeholder="Age" 
                                value={registerData.age} 
                                onChange={(e) => handleInputChange(e, 'age')} 
                            />
                        </div>
                        <div className="input-group">
                            <input 
                                type="text" 
                                placeholder="Race" 
                                value={registerData.race} 
                                onChange={(e) => handleInputChange(e, 'race')} 
                            />
                        </div>
                        <div className="input-group">
                            <input 
                                type="text" 
                                placeholder="Gender" 
                                value={registerData.gender} 
                                onChange={(e) => handleInputChange(e, 'gender')} 
                            />
                        </div>
                        <button type="submit" className="register-button">Register</button>
                    </>
                )}
            </form>
        </div>
    );
}

export default Register;
