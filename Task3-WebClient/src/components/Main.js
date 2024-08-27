import React, { useState, useEffect } from 'react';
import './Main.css';
import { useTranslation } from 'react-i18next';

function Main() {
    const { t } = useTranslation();
    const [userInfo, setUserInfo] = useState({
        name: '',
        email: '',
        phoneNumber: '',
    });

    const [currentPassword, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmNewPassword, setConfirmNewPassword] = useState('');
    const [message, setMessage] = useState('');
    const [showPasswordForm, setShowPasswordForm] = useState(false);

    useEffect(() => {
        // Предположим, что данные пользователя сохраняются в localStorage при входе
        const name = localStorage.getItem('name') || t('User');
        const email = localStorage.getItem('email') || t('Not specified');
        const phoneNumber = localStorage.getItem('phoneNumber') || t('Not specified');
        setUserInfo({ name, email, phoneNumber });
    }, [t]);

    const handlePasswordChange = async (e) => {
        e.preventDefault();

        if (newPassword !== confirmNewPassword) {
            setMessage(t('New passwords do not match'));
            return;
        }

        try {
            const response = await fetch('http://localhost:5275/auth/Auth/change-password', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`, // Передача токена авторизации
                },
                body: JSON.stringify({
                    currentPassword,
                    newPassword,
                }),
            });

            if (response.ok) {
                setMessage(t('Password successfully changed'));
                setCurrentPassword('');
                setNewPassword('');
                setConfirmNewPassword('');
                setShowPasswordForm(false); // Скрыть форму после успешной смены пароля
            } else {
                setMessage(t('Failed to change password'));
            }
        } catch (error) {
            console.error('Error changing password:', error);
            setMessage(t('An error occurred. Please try again later.'));
        }
    };

    return (
        <div className="main-container">
            <h1>{t('Welcome')}, {userInfo.name}</h1>
            <div className="user-info">
                <p>{t('Email')}: <span>{userInfo.email}</span></p>
                <p>{t('Phone Number')}: <span>{userInfo.phoneNumber}</span></p>
            </div>

            <button
                onClick={() => setShowPasswordForm(!showPasswordForm)}
                className="toggle-password-button"
            >
                {showPasswordForm ? t('Hide Password Change Form') : t('Change Password')}
            </button>

            {showPasswordForm && (
                <form onSubmit={handlePasswordChange} className="password-change-form">
                    <h3>{t('Change Password')}</h3>
                    <div className="input-group">
                        <input
                            type="password"
                            placeholder={t('Current Password')}
                            value={currentPassword}
                            onChange={(e) => setCurrentPassword(e.target.value)}
                            required
                        />
                    </div>
                    <div className="input-group">
                        <input
                            type="password"
                            placeholder={t('New Password')}
                            value={newPassword}
                            onChange={(e) => setNewPassword(e.target.value)}
                            required
                        />
                    </div>
                    <div className="input-group">
                        <input
                            type="password"
                            placeholder={t('Confirm New Password')}
                            value={confirmNewPassword}
                            onChange={(e) => setConfirmNewPassword(e.target.value)}
                            required
                        />
                    </div>
                    <button type="submit" className="change-password-button">{t('Change Password')}</button>
                    {message && <p className="message">{message}</p>}
                </form>
            )}
        </div>
    );
}

export default Main;
