import React, { useState, useEffect } from 'react';
import './App.css';  // Минимальные стили

// Types (без изменений)
interface DateTimeInfo {
  id: number;
  title: string;
  description: string;
  dateTime: string;
  timeZone: string;
  formattedDate: string;
  formattedTime: string;
  formattedDateTime: string;
}

interface ApiResponse<T> {
  data?: T;
  error?: string;
  loading: boolean;
}

const App: React.FC = () => {
  // State, API Functions, Effects, Refresh (без изменений)
  const [dateTimeEntries, setDateTimeEntries] = useState<ApiResponse<DateTimeInfo[]>>({ loading: true });
  const [currentTime, setCurrentTime] = useState<ApiResponse<DateTimeInfo>>({ loading: false });
  const [utcTime, setUtcTime] = useState<ApiResponse<DateTimeInfo>>({ loading: false });

  const fetchWithErrorHandling = async <T,>(url: string): Promise<T> => {
    const response = await fetch(url);
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return response.json();
  };

  const fetchDateTimeEntries = async () => {
    try {
      setDateTimeEntries(prev => ({ ...prev, loading: true }));
      const data = await fetchWithErrorHandling<DateTimeInfo[]>('/api/datetime');
      setDateTimeEntries({ data, loading: false });
    } catch (error) {
      setDateTimeEntries({ error: (error as Error).message, loading: false });
    }
  };

  const fetchCurrentTime = async () => {
    try {
      setCurrentTime(prev => ({ ...prev, loading: true }));
      const data = await fetchWithErrorHandling<DateTimeInfo>('/api/datetime/current');
      setCurrentTime({ data, loading: false });
    } catch (error) {
      setCurrentTime({ error: (error as Error).message, loading: false });
    }
  };

  const fetchUtcTime = async () => {
    try {
      setUtcTime(prev => ({ ...prev, loading: true }));
      const data = await fetchWithErrorHandling<DateTimeInfo>('/api/datetime/utc');
      setUtcTime({ data, loading: false });
    } catch (error) {
      setUtcTime({ error: (error as Error).message, loading: false });
    }
  };

  useEffect(() => {
    const loadData = async () => {
      await Promise.all([
        fetchDateTimeEntries(),
        fetchCurrentTime(),
        fetchUtcTime()
      ]);
    };
    loadData();
  }, []);

  const refreshAll = () => {
    fetchDateTimeEntries();
    fetchCurrentTime();
    fetchUtcTime();
  };

  const renderLoading = () => (
    <div className="loading">Загрузка...</div>
  );

  const renderError = (error: string) => (
    <div className="error">Ошибка: {error}</div>
  );

  // Render (без изменений)
  return (
    <div className="app">
      <header className="app-header">
        <h1>⏰ Сервис времени</h1>
        <p>Простой клиент API для DateTime</p>
      </header>

      <section className="section control-panel">
        <h2>Управление</h2>
        <button onClick={refreshAll} className="refresh-btn">
          Обновить все данные
        </button>
      </section>

      <section className="section">
        <h2>🕐 Текущее серверное время</h2>
        {currentTime.loading && renderLoading()}
        {currentTime.error && renderError(currentTime.error)}
        {currentTime.data && (
          <div className="time-card">
            <div className="time-display">{currentTime.data.formattedDateTime}</div>
            <div className="time-zone">{currentTime.data.timeZone}</div>
            <div className="time-description">{currentTime.data.description}</div>
          </div>
        )}
        <button onClick={fetchCurrentTime} className="refresh-btn" disabled={currentTime.loading}>
          {currentTime.loading ? 'Обновление...' : 'Обновить текущее время'}
        </button>
      </section>

      <section className="section">
        <h2>🌐 Время UTC</h2>
        {utcTime.loading && renderLoading()}
        {utcTime.error && renderError(utcTime.error)}
        {utcTime.data && (
          <div className="time-card">
            <div className="time-display">{utcTime.data.formattedDateTime}</div>
            <div className="time-zone">{utcTime.data.timeZone}</div>
            <div className="time-description">{utcTime.data.description}</div>
          </div>
        )}
        <button onClick={fetchUtcTime} className="refresh-btn" disabled={utcTime.loading}>
          {utcTime.loading ? 'Обновление...' : 'Обновить время UTC'}
        </button>
      </section>

      <section className="section">
        <div className="section-header">
          <h2>📅 Записи DateTime</h2>
          <button onClick={fetchDateTimeEntries} className="refresh-btn" disabled={dateTimeEntries.loading}>
            {dateTimeEntries.loading ? 'Обновление...' : 'Обновить записи'}
          </button>
        </div>
        {dateTimeEntries.loading && renderLoading()}
        {dateTimeEntries.error && renderError(dateTimeEntries.error)}
        {dateTimeEntries.data && (
          <div className="entries-grid">
            {dateTimeEntries.data.map(entry => (
              <div key={entry.id} className="entry-card">
                <h3>{entry.title}</h3>
                <p className="description">{entry.description}</p>
                <div className="time-info">
                  <div className="date">{entry.formattedDate}</div>
                  <div className="time">{entry.formattedTime}</div>
                  <div className="timezone">{entry.timeZone}</div>
                </div>
                <div className="entry-id">ID: {entry.id}</div>
              </div>
            ))}
          </div>
        )}
      </section>

      <footer className="status-bar">
        <div className="status-item">
          <strong>Среда:</strong> {process.env.NODE_ENV === 'development' ? 'Разработка' : 'Производство'}
        </div>
        <div className="status-item">
          <strong>Последнее обновление:</strong> {new Date().toLocaleTimeString('ru-RU')}
        </div>
      </footer>
    </div>
  );
};

export default App;
