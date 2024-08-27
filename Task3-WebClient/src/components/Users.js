import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next'; // Импортируем useTranslation
import './Users.css';
import CollapsibleSection from './CollapsibleSection';

function Users() {
    const { t } = useTranslation(); // Получаем функцию t для перевода

    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [loginInput, setLoginInput] = useState('');
    const [fetchUserIdInput, setFetchUserIdInput] = useState(''); // Для поля "Fetch User by ID"
    const [deleteUserIdInput, setDeleteUserIdInput] = useState(''); // Для поля "Delete User by ID"
    const [selectedUser, setSelectedUser] = useState(null);
    const [userError, setUserError] = useState(null);
    const [deleteUserMessage, setDeleteUserMessage] = useState('');

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await fetch('http://localhost:5275/users/User', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                });

                if (response.ok) {
                    const data = await response.json();
                    setUsers(data);
                } else {
                    setError(t('Failed to fetch users')); // Перевод ошибки
                }
            } catch (err) {
                setError(t('An error occurred while fetching users')); // Перевод ошибки
            } finally {
                setLoading(false);
            }
        };

        fetchUsers();
    }, [t]);

    const handleLoginInputChange = (e) => {
        setLoginInput(e.target.value);
    };

    const handleFetchUserIdInputChange = (e) => {
        setFetchUserIdInput(e.target.value);
    };

    const handleDeleteUserIdInputChange = (e) => {
        setDeleteUserIdInput(e.target.value);
    };

    const fetchUserByLogin = async () => {
        if (!loginInput.trim()) {
            setUserError(t('Please enter a valid login.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/users/User/bylogin/${loginInput}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const data = await response.json();
                setSelectedUser(data);
                setUserError(null);
            } else {
                setUserError(t('User not found'));
            }
        } catch (err) {
            setUserError(t('An error occurred while fetching the user'));
        }
    };

    const fetchUserById = async () => {
        if (!fetchUserIdInput.trim()) {
            setUserError(t('Please enter a valid user ID.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/users/User/${fetchUserIdInput}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const data = await response.json();
                setSelectedUser(data);
                setUserError(null);
            } else {
                setUserError(t('User not found'));
            }
        } catch (err) {
            setUserError(t('An error occurred while fetching the user'));
        }
    };

    const deleteUserById = async () => {
        if (!deleteUserIdInput.trim()) {
            setDeleteUserMessage(t('Please enter a valid user ID.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/users/User/${deleteUserIdInput}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                setDeleteUserMessage(t('User successfully deleted.'));
            } else {
                setDeleteUserMessage(t('Failed to delete user.'));
            }
        } catch (err) {
            setDeleteUserMessage(t('An error occurred while deleting the user'));
        }
    };

    return (
        <div className="page">
            <h1>{t('User Interaction Menu')}</h1>
            <CollapsibleSection title={t('Get All Users')}>
                {loading && <p>{t('Loading...')}</p>}
                {error && <p className="error">{error}</p>}
                {!loading && !error && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('User ID')}</th>
                                <th>{t('Name')}</th>
                                <th>{t('Login')}</th>
                                <th>{t('Email')}</th>
                                <th>{t('Phone')}</th>
                                <th>{t('Role')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {users.map(user => (
                                <tr key={user.userId}>
                                    <td>{user.userId}</td>
                                    <td>{user.name}</td>
                                    <td>{user.login}</td>
                                    <td>{user.email}</td>
                                    <td>{user.phoneNumber}</td>
                                    <td>{user.role}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Get User by Login')}>
                <div className="user-input-section">
                    <input
                        type="text"
                        placeholder={t('Enter login')}
                        value={loginInput}
                        onChange={handleLoginInputChange}
                    />
                    <button onClick={fetchUserByLogin}>{t('Show User')}</button>
                </div>
                {userError && <p className="error">{userError}</p>}
                {selectedUser && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('User ID')}</th>
                                <th>{t('Name')}</th>
                                <th>{t('Login')}</th>
                                <th>{t('Email')}</th>
                                <th>{t('Phone')}</th>
                                <th>{t('Role')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>{selectedUser.userId}</td>
                                <td>{selectedUser.name}</td>
                                <td>{selectedUser.login}</td>
                                <td>{selectedUser.email}</td>
                                <td>{selectedUser.phoneNumber}</td>
                                <td>{selectedUser.role}</td>
                            </tr>
                        </tbody>
                    </table>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Get User by ID')}>
                <div className="user-input-section">
                    <input
                        type="text"
                        placeholder={t('Enter User ID')}
                        value={fetchUserIdInput}
                        onChange={handleFetchUserIdInputChange}
                    />
                    <button onClick={fetchUserById}>{t('Show User')}</button>
                </div>
                {userError && <p className="error">{userError}</p>}
                {selectedUser && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('User ID')}</th>
                                <th>{t('Name')}</th>
                                <th>{t('Login')}</th>
                                <th>{t('Email')}</th>
                                <th>{t('Phone')}</th>
                                <th>{t('Role')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>{selectedUser.userId}</td>
                                <td>{selectedUser.name}</td>
                                <td>{selectedUser.login}</td>
                                <td>{selectedUser.email}</td>
                                <td>{selectedUser.phoneNumber}</td>
                                <td>{selectedUser.role}</td>
                            </tr>
                        </tbody>
                    </table>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Delete User')}>
                <div className="user-input-section">
                    <input
                        type="text"
                        placeholder={t('Enter User ID')}
                        value={deleteUserIdInput}
                        onChange={handleDeleteUserIdInputChange}
                    />
                    <button onClick={deleteUserById}>{t('Delete User')}</button>
                </div>
                {deleteUserMessage && <p className="message">{deleteUserMessage}</p>}
            </CollapsibleSection>
        </div>
    );
}

export default Users;
