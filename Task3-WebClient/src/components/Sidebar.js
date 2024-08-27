import React from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import './Sidebar.css';
import logo from '../assets/simple-red-house-hi.png';

function Sidebar({ changeLanguage }) {
    const { t } = useTranslation();

    return (
        <nav className="side-nav">
            <div className="logo-container">
                <Link to="/main">
                    <img src={logo} alt="Logo" className="logo" />
                </Link>
            </div>
            <ul>
                <li><Link to="/users">{t('Users')}</Link></li>
                <li><Link to="/groups">{t('Groups')}</Link></li>
                <li><Link to="/contents">{t('Contents')}</Link></li>
                <li><Link to="/sessions">{t('Sessions')}</Link></li>
            </ul>
            <div className="language-switcher">
                <button onClick={() => changeLanguage('en')}>EN</button>
                <button onClick={() => changeLanguage('ua')}>UA</button>
            </div>
            <button className="logout-button" onClick={() => {
                localStorage.removeItem('token');
                localStorage.removeItem('login');
                window.location.href = '/'; // Перенаправление на страницу авторизации
            }}>
                {t('Logout')}
            </button>
        </nav>
    );
}

export default Sidebar;
