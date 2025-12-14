export async function register(access_code) {
    const url = `/api/authentication/oidc?code=${encodeURIComponent(access_code)}`;
    const response = await fetch(url);

    if (!response.ok) {
        // If server returns an error status, throw it
        const errorText = await response.text();
        throw new Error(errorText);
    }

    // Only parse JSON if the response has content
    try {
        return await response.json();
    } catch {
        return null; // or {} if your API returns no content
    }
}
