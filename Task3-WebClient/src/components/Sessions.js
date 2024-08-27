import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next'; // Импортируем useTranslation
import './Sessions.css';
import CollapsibleSection from './CollapsibleSection';

function Sessions() {
    const { t } = useTranslation(); // Получаем функцию t для перевода

    const [sessions, setSessions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [groupIdInput, setGroupIdInput] = useState('');
    const [groupSessions, setGroupSessions] = useState([]);
    const [groupSessionsError, setGroupSessionsError] = useState(null);
    const [sessionIdInput, setSessionIdInput] = useState('');
    const [sessionResults, setSessionResults] = useState([]);
    const [sessionResultsError, setSessionResultsError] = useState(null);
    const [newSession, setNewSession] = useState({
        focusGroupId: '',
        contentId: '',
        startTime: '',
        endTime: ''
    });
    const [addSessionMessage, setAddSessionMessage] = useState('');

    useEffect(() => {
        const fetchSessions = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await fetch('http://localhost:5275/session/Session', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                });

                if (response.ok) {
                    const data = await response.json();
                    setSessions(data);
                } else {
                    setError(t('Failed to fetch sessions'));
                }
            } catch (err) {
                setError(t('An error occurred while fetching sessions'));
            } finally {
                setLoading(false);
            }
        };

        fetchSessions();
    }, [t]);

    const handleGroupIdInputChange = (e) => {
        setGroupIdInput(e.target.value);
    };

    const handleSessionIdInputChange = (e) => {
        setSessionIdInput(e.target.value);
    };

    const handleNewSessionInputChange = (e, field) => {
        setNewSession({ ...newSession, [field]: e.target.value });
    };

    const fetchSessionsByGroupId = async () => {
        if (!groupIdInput.trim()) {
            setGroupSessionsError(t('Please enter a valid group ID.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/session/Session/group/${groupIdInput}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const data = await response.json();
                setGroupSessions(data);
                setGroupSessionsError(null);
            } else {
                setGroupSessionsError(t('Failed to fetch sessions.'));
            }
        } catch (err) {
            setGroupSessionsError(t('An error occurred while fetching sessions.'));
        }
    };

    const fetchResultsBySessionId = async () => {
        if (!sessionIdInput.trim()) {
            setSessionResultsError(t('Please enter a valid session ID.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/result/Result/session/${sessionIdInput}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const data = await response.json();
                setSessionResults(data);
                setSessionResultsError(null);
            } else {
                setSessionResultsError(t('Failed to fetch session results.'));
            }
        } catch (err) {
            setSessionResultsError(t('An error occurred while fetching session results.'));
        }
    };

    const addNewSession = async () => {
        if (!newSession.focusGroupId || !newSession.contentId || !newSession.startTime || !newSession.endTime) {
            setAddSessionMessage(t('All fields are required.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch('http://localhost:5275/session/Session/add', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newSession)
            });

            if (response.ok) {
                setAddSessionMessage(t('Session successfully added.'));
            } else {
                setAddSessionMessage(t('Failed to add session.'));
            }
        } catch (err) {
            setAddSessionMessage(t('An error occurred while adding the session.'));
        }
    };

    return (
        <div className="sessions-page">
            <h1>{t('Sessions Interaction Menu')}</h1>

            <CollapsibleSection title={t('Get All Sessions')}>
                {loading && <p>{t('Loading...')}</p>}
                {error && <p className="error">{error}</p>}
                {!loading && !error && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('Session ID')}</th>
                                <th>{t('Focus Group ID')}</th>
                                <th>{t('Content ID')}</th>
                                <th>{t('Start Time')}</th>
                                <th>{t('End Time')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {sessions.map(session => (
                                <tr key={session.sessionId}>
                                    <td>{session.sessionId}</td>
                                    <td>{session.focusGroupId}</td>
                                    <td>{session.contentId}</td>
                                    <td>{new Date(session.startTime).toLocaleString()}</td>
                                    <td>{new Date(session.endTime).toLocaleString()}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Get Sessions by Focus Group ID')}>
                <div className="group-sessions-section">
                    <input
                        type="text"
                        placeholder={t('Enter Focus Group ID')}
                        value={groupIdInput}
                        onChange={handleGroupIdInputChange}
                    />
                    <button onClick={fetchSessionsByGroupId}>{t('Show Sessions')}</button>
                </div>
                {groupSessionsError && <p className="error">{groupSessionsError}</p>}
                {!groupSessionsError && groupSessions.length > 0 && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('Session ID')}</th>
                                <th>{t('Focus Group ID')}</th>
                                <th>{t('Content ID')}</th>
                                <th>{t('Start Time')}</th>
                                <th>{t('End Time')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {groupSessions.map(session => (
                                <tr key={session.sessionId}>
                                    <td>{session.sessionId}</td>
                                    <td>{session.focusGroupId}</td>
                                    <td>{session.contentId}</td>
                                    <td>{new Date(session.startTime).toLocaleString()}</td>
                                    <td>{new Date(session.endTime).toLocaleString()}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
                {!groupSessionsError && groupSessions.length === 0 && (
                    <p>{t('No sessions found for this focus group.')}</p>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Get Session Analysis Results')}>
                <div className="session-results-section">
                    <input
                        type="text"
                        placeholder={t('Enter Session ID')}
                        value={sessionIdInput}
                        onChange={handleSessionIdInputChange}
                    />
                    <button onClick={fetchResultsBySessionId}>{t('Show Analysis Results')}</button>
                </div>
                {sessionResultsError && <p className="error">{sessionResultsError}</p>}
                {!sessionResultsError && sessionResults.length > 0 && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('Analysis Result')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {sessionResults.map(result => (
                                <tr key={result.resultId}>
                                    <td>{result.avrEmotion}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Add New Session')}>
                <div className="add-session-section">
                    <input
                        type="text"
                        placeholder={t('Enter Focus Group ID')}
                        value={newSession.focusGroupId}
                        onChange={(e) => handleNewSessionInputChange(e, 'focusGroupId')}
                    />
                    <input
                        type="text"
                        placeholder={t('Enter Content ID')}
                        value={newSession.contentId}
                        onChange={(e) => handleNewSessionInputChange(e, 'contentId')}
                    />
                    <input
                        type="datetime-local"
                        placeholder={t('Start Time')}
                        value={newSession.startTime}
                        onChange={(e) => handleNewSessionInputChange(e, 'startTime')}
                    />
                    <input
                        type="datetime-local"
                        placeholder={t('End Time')}
                        value={newSession.endTime}
                        onChange={(e) => handleNewSessionInputChange(e, 'endTime')}
                    />
                    <button onClick={addNewSession}>{t('Add Session')}</button>
                </div>
                {addSessionMessage && <p className="message">{addSessionMessage}</p>}
            </CollapsibleSection>
        </div>
    );
}

export default Sessions;
