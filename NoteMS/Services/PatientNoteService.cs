using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoteMS.Models;

namespace NoteMS.Services
{
    public class PatientNoteService
    {
        private readonly IMongoCollection<Note> _patientNoteCollection;

        public PatientNoteService(
            IOptions<PatientNoteStoreDatabaseSettings> patientNoteStoreDataseSettings)
        {
            var mongoClient = new MongoClient(
                patientNoteStoreDataseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                patientNoteStoreDataseSettings.Value.DatabaseName);

            _patientNoteCollection = mongoDatabase.GetCollection<Note>(
                patientNoteStoreDataseSettings.Value.PatientNoteCollectionName);
        }

        public async Task<List<Note>> GetAsync() =>
            await _patientNoteCollection.Find(_ => true).ToListAsync();

        public async Task<Note?> GetAsync(string id) =>
            await _patientNoteCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Note newNote) =>
            await _patientNoteCollection.InsertOneAsync(newNote);

        public async Task UpdateAsync(string id, Note updatedNote) =>
            await _patientNoteCollection.ReplaceOneAsync(x => x.Id == id, updatedNote);

        public async Task RemoveAsync(string id) =>
            await _patientNoteCollection.DeleteOneAsync(x => x.Id == id);
    }
}
