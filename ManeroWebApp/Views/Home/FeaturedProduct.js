async function featuredProducts() {
    try {
        const response = await fetch('');
        const data = await response.json();
        const productsEl = document.getElementById('product-grid');
        for (let i = 0; i < data.length; i++) {
            const product = data[i];
            const productEl = document.createElement('div');
            productEl.classList.add('product-card');

            const rating = product.starRating;
            const filledStar = '<div class="star">&#9733;</div>';
            const emptyStar = '<div class="star">&#9734;</div>';
            const ratingProduct = filledStar.repeat(rating) + emptyStar.repeat(5 - rating);

            productEl.innerHTML = `
            <div class="product-image">
                <img src="${product.imageUrl}" alt="${product.name}">
                <div class="product-icons">
                    <i class="fa-regular fa-heart"></i>
                    <i class="fa-regular fa-bag-shopping"></i>
                </div>
            </div>
            <div class="product-details">
                <div class="product-review">
                    ${ratingProduct}
                    <span class="review-score">${product.starRating}</span>
                </div>
                <div class="product-name">${product.name}</div>
                <div class="product-price">${product.originalPrice} ${product.currency}</div>
            </div>`;

            productsEl.appendChild(productEl);
        }
    } catch (error) {
        console.error('Error fetching products:', error);
    }
}

featuredProducts();
