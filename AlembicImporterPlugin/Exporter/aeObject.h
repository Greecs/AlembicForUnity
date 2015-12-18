#ifndef aeObject_h
#define aeObject_h

class aeProperty
{
public:
    aeProperty();
    virtual ~aeProperty();
    virtual void writeSample(const void *data, int data_num) = 0;
};


class aeObject
{
public:
    aeObject(aeContext *ctx, aeObject *parent, AbcGeom::OObject *abc);
    virtual ~aeObject();

    const char* getName() const;
    const char* getFullName() const;
    uint32_t    getNumChildren() const;
    aeObject*   getChild(int i);
    aeObject*   getParent();

    aeContext*          getContext();
    const aeConfig&     getConfig() const;
    virtual abcObject&  getAbcObject();
    virtual abcProperties* getAbcProperties();

    /// T: aeCamera, aeXForm, aePoint, aePolyMesh
    template<class T> T*    newChild(const char *name);
    void                    removeChild(aeObject *c);

    /// T: Abc::OFloatArrayProperty, Abc::OV2fArrayProperty, etc
    template<class T>
    aeProperty*             newProperty(const char *name);

protected:
    typedef std::unique_ptr<aeProperty> aePropertyPtr;
    aeContext                   *m_ctx;
    aeObject                    *m_parent;
    std::unique_ptr<abcObject>  m_abc;
    std::vector<aePropertyPtr>  m_properties;
    std::vector<aeObject*>      m_children;
};

#endif // aeObject_h
