export class FileUploadService {
    constructor(private containerSasUrl: string) {}
  
    async uploadPdf(archivoBlob: Blob): Promise<void> {
      const response = await fetch(this.containerSasUrl, {
        method: "PUT",
        headers: {
          "x-ms-blob-type": "BlockBlob",
          "Content-Type": "application/pdf",
        },
        body: archivoBlob,
      });
  
      if (!response.ok) {
        const error = await response.text();
        throw new Error(`Error ${response.status}: ${error}`);
      }
    }
  }