




function refreshCartCounter() {    
    fetch('/Cart/CartNavPartial')
        .then(res => res.text())
        .then(html => {
            const container = document.getElementById('cart-summary');
            
            if (container) {
                container.innerHTML = '';
                container.insertAdjacentHTML('afterbegin', html);
            }
        })
        .catch(err => console.error("Failed to update cart", err));
}

async function addToCart(productId) {
    const res = await fetch('/api/cart/increase/'+productId, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({ productId })
    });

    if (res.ok) {
        refreshCartCounter();
    }
}

async function removeFromCart(productId) {
    const res = await fetch('/api/cart/decrease'+productId, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({ productId })
    });

    if (res.ok) {
        refreshCartCounter();
    }
}
