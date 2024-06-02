export const fetchAndConvertImage = async (url) => {
    try {
      const response = await fetch(url);
      const contentType = response.headers.get('content-type');

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      if (contentType !== 'image/jpeg' && contentType !== 'image/png') {
        throw new Error('Image format not supported. Only .jpg and .png are allowed.');
      }

      const blob = await response.blob();
      const extension = contentType.split('/')[1];
      const fileName = `image.${extension}`;
      const file = new File([blob], fileName, { type: contentType });
      return file;
    } catch (error) {
      console.error('Error fetching or converting image:', error);
      throw new Error("Couldn't convert images");
    }
};