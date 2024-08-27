import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import Main from './components/Main';
import Users from './components/Users';
import Groups from './components/Groups';
import Contents from './components/Contents';
import Sessions from './components/Sessions';
import Login from './components/Login';
import Sidebar from './components/Sidebar';
import Register from './components/Register';
import './components/i18n';

function App() {
    const { i18n } = useTranslation();

    const changeLanguage = (lng) => {
        i18n.changeLanguage(lng);
    };

    return (
        <Router>
            <div className="app-container">
                <Routes>
                    <Route path="/" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route
                        path="*"
                        element={
                            <>
                                <Sidebar changeLanguage={changeLanguage} />
                                <div className="main-content">
                                    <Routes>
                                        <Route path="/main" element={<Main />} />
                                        <Route path="/users" element={<Users />} />
                                        <Route path="/groups" element={<Groups />} />
                                        <Route path="/contents" element={<Contents />} />
                                        <Route path="/sessions" element={<Sessions />} />
                                    </Routes>
                                </div>
                            </>
                        }
                    />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
