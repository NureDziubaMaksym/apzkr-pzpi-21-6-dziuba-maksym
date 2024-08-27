import React, { useState } from 'react';
import './CollapsibleSection.css';

function CollapsibleSection({ title, children }) {
    const [isCollapsed, setIsCollapsed] = useState(true);

    const toggleCollapse = () => {
        console.log('Toggling collapse:', !isCollapsed);
        setIsCollapsed(!isCollapsed);
    };
    

    return (
        <div className="collapsible-section">
            <div className="section-header" onClick={toggleCollapse}>
                <h2>{title}</h2>
                <button className="toggle-button">
                    {isCollapsed ? '+' : '-'}
                </button>
            </div>
            <div className={`section-content ${isCollapsed ? 'collapsed' : 'expanded'}`}>
                {isCollapsed ? null : children}
            </div>
        </div>
    );
}

export default CollapsibleSection;
