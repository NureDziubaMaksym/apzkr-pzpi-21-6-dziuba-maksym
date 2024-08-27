import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next'; // Импортируем useTranslation
import './Groups.css';
import CollapsibleSection from './CollapsibleSection';

function Groups() {
    const { t } = useTranslation(); // Используем t для перевода

    const [groups, setGroups] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [fetchGroupIdInput, setFetchGroupIdInput] = useState(''); // Input field for group ID to fetch users
    const [addUserGroupIdInput, setAddUserGroupIdInput] = useState(''); // Input field for group ID to add a user
    const [userIdInput, setUserIdInput] = useState(''); // Input field for userID
    const [removeUserGroupIdInput, setRemoveUserGroupIdInput] = useState(''); // Input field to remove user from group
    const [removeUserIdInput, setRemoveUserIdInput] = useState(''); // Input field to remove user from group
    const [newGroupRaceInput, setNewGroupRaceInput] = useState(''); // Input field for new group's race
    const [newGroupAgeInput, setNewGroupAgeInput] = useState(''); // Input field for new group's age
    const [newGroupGenderInput, setNewGroupGenderInput] = useState(''); // Input field for new group's gender
    const [deleteGroupIdInput, setDeleteGroupIdInput] = useState(''); // Input field to delete a group
    const [users, setUsers] = useState([]);
    const [usersLoading, setUsersLoading] = useState(false);
    const [usersError, setUsersError] = useState(null);
    const [addUserMessage, setAddUserMessage] = useState(''); // Message for success or error in adding a user
    const [removeUserMessage, setRemoveUserMessage] = useState(''); // Message for success or error in removing a user
    const [addGroupMessage, setAddGroupMessage] = useState(''); // Message for success or error in adding a group
    const [deleteGroupMessage, setDeleteGroupMessage] = useState(''); // Message for success or error in deleting a group

    useEffect(() => {
        const fetchGroups = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await fetch('http://localhost:5275/groups/FocusGroup', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                });

                if (response.ok) {
                    const data = await response.json();
                    setGroups(data);
                } else {
                    setError(t('Failed to fetch groups'));
                }
            } catch (err) {
                setError(t('An error occurred while fetching groups'));
            } finally {
                setLoading(false);
            }
        };

        fetchGroups();
    }, [t]);

    const fetchUsersByGroupId = async (groupId) => {
        setUsersLoading(true);
        setUsersError(null);
        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/groups/FocusGroup/${groupId}/users`, {
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
                setUsersError(t('Failed to fetch users'));
            }
        } catch (err) {
            setUsersError(t('An error occurred while fetching users'));
        } finally {
            setUsersLoading(false);
        }
    };

    const handleFetchGroupIdInputChange = (event) => {
        setFetchGroupIdInput(event.target.value); // Update state for group ID search
    };

    const handleAddUserGroupIdInputChange = (event) => {
        setAddUserGroupIdInput(event.target.value); // Update state for adding user by group ID
    };

    const handleUserIdInputChange = (event) => {
        setUserIdInput(event.target.value); // Update state for user ID input
    };

    const handleRemoveUserGroupIdInputChange = (event) => {
        setRemoveUserGroupIdInput(event.target.value); // Update state for removing user by group ID
    };

    const handleRemoveUserIdInputChange = (event) => {
        setRemoveUserIdInput(event.target.value); // Update state for removing user by user ID
    };

    const handleNewGroupRaceInputChange = (event) => {
        setNewGroupRaceInput(event.target.value); // Update state for new group's race
    };

    const handleNewGroupAgeInputChange = (event) => {
        setNewGroupAgeInput(event.target.value); // Update state for new group's age
    };

    const handleNewGroupGenderInputChange = (event) => {
        setNewGroupGenderInput(event.target.value); // Update state for new group's gender
    };

    const handleDeleteGroupIdInputChange = (event) => {
        setDeleteGroupIdInput(event.target.value); // Update state for group deletion
    };

    const handleFetchUsers = () => {
        if (fetchGroupIdInput.trim()) {
            fetchUsersByGroupId(fetchGroupIdInput); // Fetch users by entered group ID
        } else {
            setUsersError(t('Please enter a valid group ID'));
        }
    };

    const handleAddUserToGroup = async () => {
        if (!userIdInput.trim() || !addUserGroupIdInput.trim()) {
            setAddUserMessage(t('Both User ID and Group ID are required.'));
            return;
        }

        const body = {
            userId: parseInt(userIdInput, 10),
            groupId: parseInt(addUserGroupIdInput, 10)
        };

        try {
            const token = localStorage.getItem('token');
            const response = await fetch('http://localhost:5275/groups/FocusGroup/addmember', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(body)
            });

            if (response.ok) {
                setAddUserMessage(t('User successfully added to the group.'));
            } else {
                setAddUserMessage(t('Failed to add user to the group.'));
            }
        } catch (err) {
            setAddUserMessage(t('An error occurred while adding the user to the group.'));
        }
    };

    const handleRemoveUserFromGroup = async () => {
        if (!removeUserGroupIdInput.trim() || !removeUserIdInput.trim()) {
            setRemoveUserMessage(t('Both Group ID and User ID are required.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/groups/FocusGroup/removeuser/${removeUserGroupIdInput}/${removeUserIdInput}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                setRemoveUserMessage(t('User successfully removed from the group.'));
            } else {
                setRemoveUserMessage(t('Failed to remove user from the group.'));
            }
        } catch (err) {
            setRemoveUserMessage(t('An error occurred while removing the user from the group.'));
        }
    };

    const handleAddGroup = async () => {
        if (!newGroupRaceInput.trim() || !newGroupAgeInput.trim() || !newGroupGenderInput.trim()) {
            setAddGroupMessage(t('Race, Age, and Gender are required.'));
            return;
        }

        const body = {
            race: newGroupRaceInput,
            age: newGroupAgeInput,
            gender: newGroupGenderInput
        };

        try {
            const token = localStorage.getItem('token');
            const response = await fetch('http://localhost:5275/groups/FocusGroup/add', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(body)
            });

            if (response.ok) {
                setAddGroupMessage(t('Group successfully added.'));
            } else {
                setAddGroupMessage(t('Failed to add group.'));
            }
        } catch (err) {
            setAddGroupMessage(t('An error occurred while adding the group.'));
        }
    };

    const handleDeleteGroup = async () => {
        if (!deleteGroupIdInput.trim()) {
            setDeleteGroupMessage(t('Group ID is required.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/groups/FocusGroup/${deleteGroupIdInput}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                setDeleteGroupMessage(t('Group successfully deleted.'));
            } else {
                setDeleteGroupMessage(t('Failed to delete group.'));
            }
        } catch (err) {
            setDeleteGroupMessage(t('An error occurred while deleting the group.'));
        }
    };

    return (
        <div className="groups-page">
            <h1>{t('Focus Group Interaction Menu')}</h1>
            <CollapsibleSection title={t('Get All Focus Groups')}>
                {loading && <p>{t('Loading...')}</p>}
                {error && <p className="error">{error}</p>}
                {!loading && !error && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('Group ID')}</th>
                                <th>{t('Race')}</th>
                                <th>{t('Age')}</th>
                                <th>{t('Gender')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {groups.map(group => (
                                <tr key={group.focGrId}>
                                    <td>{group.focGrId}</td>
                                    <td>{group.race}</td>
                                    <td>{group.age}</td>
                                    <td>{group.gender}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Get Users by Group ID')}>
                <div className="user-input-section">
                    <input
                        type="text"
                        placeholder={t('Enter Group ID')}
                        value={fetchGroupIdInput}
                        onChange={handleFetchGroupIdInputChange}
                    />
                    <button onClick={handleFetchUsers}>{t('Show Users')}</button>
                </div>
                {usersLoading && <p>{t('Loading users...')}</p>}
                {usersError && <p className="error">{usersError}</p>}
                {!usersLoading && !usersError && users.length > 0 && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('User ID')}</th>
                                <th>{t('Name')}</th>
                                <th>{t('Email')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {users.map(user => (
                                <tr key={user.id}>
                                    <td>{user.userId}</td>
                                    <td>{user.name}</td>
                                    <td>{user.email}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
                {!usersLoading && !usersError && users.length === 0 && (
                    <p>{t('No users in this group.')}</p>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Add User to Focus Group')}>
                <div className="add-user-section">
                    <input
                        type="text"
                        placeholder={t('Enter User ID')}
                        value={userIdInput}
                        onChange={handleUserIdInputChange}
                    />
                    <input
                        type="text"
                        placeholder={t('Enter Group ID')}
                        value={addUserGroupIdInput}
                        onChange={handleAddUserGroupIdInputChange}
                    />
                    <button onClick={handleAddUserToGroup}>{t('Add User')}</button>
                </div>
                {addUserMessage && <p className="message">{addUserMessage}</p>}
            </CollapsibleSection>

            <CollapsibleSection title={t('Remove User from Focus Group')}>
                <div className="remove-user-section">
                    <input
                        type="text"
                        placeholder={t('Enter Group ID')}
                        value={removeUserGroupIdInput}
                        onChange={handleRemoveUserGroupIdInputChange}
                    />
                    <input
                        type="text"
                        placeholder={t('Enter User ID')}
                        value={removeUserIdInput}
                        onChange={handleRemoveUserIdInputChange}
                    />
                    <button onClick={handleRemoveUserFromGroup}>{t('Remove User')}</button>
                </div>
                {removeUserMessage && <p className="message">{removeUserMessage}</p>}
            </CollapsibleSection>

            <CollapsibleSection title={t('Add New Focus Group')}>
                <div className="add-group-section">
                    <input
                        type="text"
                        placeholder={t('Enter Race')}
                        value={newGroupRaceInput}
                        onChange={handleNewGroupRaceInputChange}
                    />
                    <input
                        type="text"
                        placeholder={t('Enter Age')}
                        value={newGroupAgeInput}
                        onChange={handleNewGroupAgeInputChange}
                    />
                    <input
                        type="text"
                        placeholder={t('Enter Gender')}
                        value={newGroupGenderInput}
                        onChange={handleNewGroupGenderInputChange}
                    />
                    <button onClick={handleAddGroup}>{t('Add Group')}</button>
                </div>
                {addGroupMessage && <p className="message">{addGroupMessage}</p>}
            </CollapsibleSection>

            <CollapsibleSection title={t('Delete Focus Group')}>
                <div className="delete-group-section">
                    <input
                        type="text"
                        placeholder={t('Enter Group ID')}
                        value={deleteGroupIdInput}
                        onChange={handleDeleteGroupIdInputChange}
                    />
                    <button onClick={handleDeleteGroup}>{t('Delete Group')}</button>
                </div>
                {deleteGroupMessage && <p className="message">{deleteGroupMessage}</p>}
            </CollapsibleSection>
        </div>
    );
}

export default Groups;
