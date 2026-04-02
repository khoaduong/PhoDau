// TaskRequest model to match Backend TaskItem structure
export class TaskRequest {
  constructor(id, title, description, isCompleted, createdAt) {
    this.id = id; // string (GUID)
    this.title = title; // string
    this.description = description; // string or null
    this.isCompleted = isCompleted; // boolean
    this.createdAt = createdAt; // Date
  }

  // Static method to create from object
  static fromObject(obj) {
    return new TaskRequest(
      obj.id,
      obj.title,
      obj.description,
      obj.isCompleted,
      new Date(obj.createdAt)
    );
  }

  // Method to convert to plain object for API
  toObject() {
    return {
      id: this.id,
      title: this.title,
      description: this.description,
      isCompleted: this.isCompleted,
      createdAt: this.createdAt.toISOString()
    };
  }
}