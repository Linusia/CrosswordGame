document.addEventListener('DOMContentLoaded', function () {
    const wordInput = document.getElementById('word-input');
    const checkWordBtn = document.getElementById('check-word');
    const clearWordBtn = document.getElementById('clear-word');
    const lettersContainer = document.getElementById('letters-container');
    const foundWordsContainer = document.getElementById('found-words');
    const messageArea = document.getElementById('message-area');

    let availableLetters = [];
    let foundWords = [];

    function initGame() {
        loadGameState();
        updateUI();
    }

    function loadGameState() {
        availableLetters = Array.from('AKOTMLPYS');
        foundWords = [];
        updateLettersDisplay();
        updateFoundWords();
    }

    function updateLettersDisplay() {
        lettersContainer.innerHTML = '';
        availableLetters.forEach(letter => {
            const letterElement = document.createElement('span');
            letterElement.className = 'badge bg-primary fs-4 p-3';
            letterElement.textContent = letter;
            letterElement.style.cursor = 'pointer';
            letterElement.addEventListener('click', () => {
                wordInput.value += letter;
            });
            lettersContainer.appendChild(letterElement);
        });
    }

    function updateFoundWords() {
        foundWordsContainer.innerHTML = '';
        if (foundWords.length === 0) {
            foundWordsContainer.innerHTML = '<p class="text-muted">Brak znalezionych słów</p>';
        } else {
            foundWords.forEach(word => {
                const wordElement = document.createElement('div');
                wordElement.className = 'badge bg-success fs-5 m-1';
                wordElement.textContent = word;
                foundWordsContainer.appendChild(wordElement);
            });
        }
    }

    function showMessage(message, isSuccess) {
        messageArea.innerHTML = `
            <div class="alert ${isSuccess ? 'alert-success' : 'alert-danger'} alert-dismissible fade show" role="alert">
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `;

        setTimeout(() => {
            const alert = document.querySelector('.alert');
            if (alert) {
                alert.remove();
            }
        }, 3000);
    }

    function checkWord() {
        const word = wordInput.value.trim().toUpperCase();

        if (word.length === 0) {
            showMessage('Wpisz słowo!', false);
            return;
        }

        fetch('/Home/CheckWord', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `word=${encodeURIComponent(word)}`
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    showMessage(data.message, true);
                    availableLetters = Array.from(data.availableLetters);
                    foundWords = data.foundWords;
                    updateLettersDisplay();
                    updateFoundWords();
                    wordInput.value = '';
                } else {
                    showMessage(data.message, false);
                }
            })
            .catch(error => {
                console.error('Błąd:', error);
                showMessage('Wystąpił błąd podczas sprawdzania słowa', false);
            });
    }

    function clearWord() {
        wordInput.value = '';
    }

    checkWordBtn.addEventListener('click', checkWord);
    clearWordBtn.addEventListener('click', clearWord);

    wordInput.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') {
            checkWord();
        }
    });

    initGame();
});