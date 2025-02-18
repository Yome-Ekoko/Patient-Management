# Patient Management System

## Project Overview
The Patient Management System (PMS) is a healthcare-oriented platform designed to streamline patient data management, appointment scheduling, and record-keeping. The system ensures secure, role-based access control, enabling doctors, administrators, and patients to interact efficiently.

## Thought Process and Design Decisions

### 1. **User Registration and Role Assignment**
- Every individual registers first as a general **User**.
- After registration, a user can be assigned registered or created into specific roles:
  - **Admin** (manually created and assigned by an existing admin)
  - **Doctor** (registered users can be promoted to doctors)
  - **Patient** (users become patients upon creation in the system)
- This approach ensures flexibility in role assignment and prevents unauthorized access to role-specific features.

### 2. **System Architecture**
- **Backend:** .NET Core with Entity Framework
- **Database:** SQLite (or another relational/document database based on the environment)
- **Frontend:** Not included in this phase, but a future extension can incorporate React/Angular
- **Authentication:** Identity-based authentication with role-based access control
- **Dependency Injection:** Ensuring modular and testable components

### 3. **Key Modules and Implementation**
#### a. **User Management**
- Users register and authenticate via JWT authentication.
- Role-based access control determines user privileges.

#### b. **Patient Management**
- Patients have associated **PatientRecords**, which store historical medical data.
- A soft delete mechanism ensures data integrity.

#### c. **Doctor Management**
- Doctors are assigned specific patients and can update patient records.
- The system ensures that only active users can perform actions.
- This feature is to be implemented at a later sprint as the goal is to achieve an MVP.

#### d. **Appointments**
- Patients can book appointments with available doctors.
- Doctors and admins can manage appointment schedules.

#### e. **Patient Records Management**
- Each patient has multiple records that track medical history.
- Patient records include timestamps and are linked to appointments.
- When fetching a patient, the system can also retrieve the **list of patient records**.
- When retrieving a **patient record**, the system includes the **list of related appointments** for better context.

### 4. **API Design Considerations**
- **Service Layer:** Encapsulates business logic to keep controllers thin.
- **Repository Pattern:** Ensures abstraction from direct database operations.
- **Paging & Filtering:** Implemented in endpoints that return large datasets (e.g., patient records, appointments).
- **Error Handling:** Custom exceptions for clear API responses.

## Future Enhancements
- **Real-time Notifications:** Integration with SignalR for instant updates.
- **Advanced Search & Filters:** More powerful queries for patient records.
- **Integration with External Systems:** Support for HL7/FHIR standards.
- **Role-based UI Components:** Tailored UI for different users.

## Conclusion
This system is built with scalability, security, and usability in mind. The clear separation of concerns, modular structure, and extensible architecture ensure that it can evolve with future healthcare needs.

