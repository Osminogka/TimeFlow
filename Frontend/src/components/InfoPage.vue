<template>
    <div class="block">
        <div class="info-block">
            <h2>📊 Take Control of Your Budget</h2>
            <p>Managing your finances has never been easier! Track your expenses across different categories like food, transport, entertainment, and more. 
            Get insights into your spending habits and make better financial decisions.
            </p>
        </div>
        <div class="info-block">
            <h2>👥 Connect with Friends</h2>
            <p>Your finances, your way! Set your budget stats to private or public and share insights with friends. 
            Send friend requests, see their budgeting habits, and support each other in reaching financial goals.
            </p>
        </div>
        <div class="info-block">
            <h2>🚀 Why Choose TimeFlow?</h2>
            <ul>
                <li>✅ <strong>Easy-to-Use:</strong> Simple and intuitive design</li>
                <li>✅ <strong>Detailed Analytics:</strong> Visualize your spending trends</li>
                <li>✅ <strong>Social Features:</strong> Connect with friends and stay motivated</li>
                <li>✅ <strong>Privacy First:</strong> Choose what you share and with whom</li>
            </ul>
            <p class="cta">🔹 Ready to get started?</p> 
            <p class="cta"><router-link class="signup-link" :to="{ name: 'Register' }">Get Started!</router-link></p>
        </div>
    </div>
</template>


<style scoped>

.block {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: 1.5rem;
}

.info-block {
    flex: 1;
    max-width: 800px;
    padding: 0.5rem;
    border-radius: 10px;
    background: linear-gradient(135deg, #fc92d3, #f373b9);
    color: #fff;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
}

.info-block:hover {
    transform: scale(1.05);
    box-shadow: 0 6px 15px rgba(0, 0, 0, 0.15);
}

h2 {
    margin-bottom: 0.5rem;
    font-size: 1.5rem;
    text-align: center;
}

p, li {
    font-size: 1rem;
    line-height: 1.6;
}

ul {
    padding-left: 1rem;
}

.cta {
    font-weight: bold;
    text-align: center;
    margin-top: 1rem;
}

.signup-link {
    color: #4437a3;
    text-decoration: none;
    font-weight: bold;
}

.signup-link:hover {
    text-decoration: underline;
}

@media (max-width: 768px) {
    .block {
        flex-direction: column;
        align-items: center;
    }

    .info-block {
        width: 90%;
    }
}

@media (min-width: 768px) {
    .info-block {
        min-width: 800px;
    }
}
</style>
