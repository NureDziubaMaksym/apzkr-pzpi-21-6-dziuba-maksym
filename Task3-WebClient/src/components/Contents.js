import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next'; // Импортируем useTranslation
import './Contents.css';
import CollapsibleSection from './CollapsibleSection';

function Contents() {
    const { t } = useTranslation(); // Получаем функцию t для перевода

    const [contents, setContents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [newContent, setNewContent] = useState({ title: '', type: '', description: '', url: '' });
    const [addContentMessage, setAddContentMessage] = useState('');
    const [reactionContentId, setReactionContentId] = useState('');
    const [reactions, setReactions] = useState([]);
    const [reactionsError, setReactionsError] = useState(null);
    const [analysisContentId, setAnalysisContentId] = useState('');
    const [analysisResults, setAnalysisResults] = useState([]);
    const [analysisError, setAnalysisError] = useState(null);

    useEffect(() => {
        const fetchContents = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await fetch('http://localhost:5275/content/Content', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                });

                if (response.ok) {
                    const data = await response.json();
                    setContents(data);
                } else {
                    setError(t('Failed to fetch contents')); // Перевод ошибки
                }
            } catch (err) {
                setError(t('An error occurred while fetching contents')); // Перевод ошибки
            } finally {
                setLoading(false);
            }
        };

        fetchContents();
    }, [t]);

    const handleInputChange = (event, field) => {
        setNewContent({ ...newContent, [field]: event.target.value });
    };

    const handleReactionContentIdChange = (event) => {
        setReactionContentId(event.target.value);
    };

    const handleAnalysisContentIdChange = (event) => {
        setAnalysisContentId(event.target.value);
    };

    const addNewContent = async () => {
        if (!newContent.title || !newContent.type || !newContent.description || !newContent.url) {
            setAddContentMessage(t('All fields are required.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch('http://localhost:5275/content/Content', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newContent)
            });

            if (response.ok) {
                setAddContentMessage(t('Content successfully added.'));
            } else {
                setAddContentMessage(t('Failed to add content.'));
            }
        } catch (err) {
            setAddContentMessage(t('An error occurred while adding content.'));
        }
    };

    const fetchReactionsByContentId = async () => {
        if (!reactionContentId.trim()) {
            setReactionsError(t('Please enter a valid content ID.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/reactions/Reaction/content/${reactionContentId}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const data = await response.json();
                setReactions(data);
                setReactionsError(null);
            } else {
                setReactionsError(t('Failed to fetch reactions.'));
            }
        } catch (err) {
            setReactionsError(t('An error occurred while fetching reactions.'));
        }
    };

    const fetchAnalysisByContentId = async () => {
        if (!analysisContentId.trim()) {
            setAnalysisError(t('Please enter a valid content ID.'));
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`http://localhost:5275/result/Result/content/${analysisContentId}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const data = await response.json();
                setAnalysisResults(data);
                setAnalysisError(null);
            } else {
                setAnalysisError(t('Failed to fetch analysis results.'));
            }
        } catch (err) {
            setAnalysisError(t('An error occurred while fetching analysis results.'));
        }
    };

    return (
        <div className="page">
            <h1>{t('Content Interaction Menu')}</h1>
            <CollapsibleSection title={t('Get all content')}>
                {loading && <p>{t('Loading...')}</p>}
                {error && <p className="error">{error}</p>}
                {!loading && !error && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('ID Content')}</th>
                                <th>{t('Title')}</th>
                                <th>{t('Type')}</th>
                                <th>{t('Description')}</th>
                                <th>{t('URL')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {contents.map(content => (
                                <tr key={content.contentId}>
                                    <td>{content.contentId}</td>
                                    <td>{content.title}</td>
                                    <td>{content.type}</td>
                                    <td>{content.description}</td>
                                    <td>
                                        <a href={content.url} target="_blank" rel="noopener noreferrer">
                                            {t('Go to')}
                                        </a>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Add new content')}>
                <div className="add-content-section">
                    <input
                        type="text"
                        placeholder={t('Enter title')}
                        value={newContent.title}
                        onChange={(e) => handleInputChange(e, 'title')}
                    />
                    <input
                        type="text"
                        placeholder={t('Enter type')}
                        value={newContent.type}
                        onChange={(e) => handleInputChange(e, 'type')}
                    />
                    <input
                        type="text"
                        placeholder={t('Enter description')}
                        value={newContent.description}
                        onChange={(e) => handleInputChange(e, 'description')}
                    />
                    <input
                        type="text"
                        placeholder={t('Enter URL')}
                        value={newContent.url}
                        onChange={(e) => handleInputChange(e, 'url')}
                    />
                    <button onClick={addNewContent}>{t('Add Content')}</button>
                </div>
                {addContentMessage && <p className="message">{addContentMessage}</p>}
            </CollapsibleSection>

            <CollapsibleSection title={t('Get user reactions to content')}>
                <div className="reaction-content-section">
                    <input
                        type="text"
                        placeholder={t('Enter ID content')}
                        value={reactionContentId}
                        onChange={handleReactionContentIdChange}
                    />
                    <button onClick={fetchReactionsByContentId}>{t('Show Reactions')}</button>
                </div>
                {reactionsError && <p className="error">{reactionsError}</p>}
                {!reactionsError && reactions.length > 0 && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('ID Reaction')}</th>
                                <th>{t('ID User')}</th>
                                <th>{t('Grade')}</th>
                                <th>{t('Level Interest')}</th>
                                <th>{t('Comment')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {reactions.map(reaction => (
                                <tr key={reaction.reactionId}>
                                    <td>{reaction.reactionId}</td>
                                    <td>{reaction.userId}</td>
                                    <td>{reaction.grade}</td>
                                    <td>{reaction.interestRate}</td>
                                    <td>{reaction.commentText}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </CollapsibleSection>

            <CollapsibleSection title={t('Get content analysis results')}>
                <div className="analysis-content-section">
                    <input
                        type="text"
                        placeholder={t('Enter ID content')}
                        value={analysisContentId}
                        onChange={handleAnalysisContentIdChange}
                    />
                    <button onClick={fetchAnalysisByContentId}>{t('Show Analysis Results')}</button>
                </div>
                {analysisError && <p className="error">{analysisError}</p>}
                {!analysisError && analysisResults.length > 0 && (
                    <table>
                        <thead>
                            <tr>
                                <th>{t('Analysis Result')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {analysisResults.map(result => (
                                <tr key={result.resultId}>
                                    <td>{result.avrEmotion}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </CollapsibleSection>
        </div>
    );
}

export default Contents;
